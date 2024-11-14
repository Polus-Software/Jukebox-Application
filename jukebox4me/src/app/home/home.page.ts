import { Component } from '@angular/core';
import { ApiService } from '../services/api.service'
import { AlertController, ToastController } from '@ionic/angular';
import { GetNowPlayingResponse, GetSongsResponse, Track, NowPlayingSong } from './home.page.interface';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-home',
    templateUrl: 'home.page.html',
    styleUrls: ['home.page.scss'],
})
export class HomePage {

    playlist: Track[] = [];
    selectedSegment: string = 'all';
    searchBarVisible: boolean = true;
    nowPlaying: NowPlayingSong = {
        title: '',
        id: '',
        albumTitle: '',
        thumbnailUrl: '',
        artists: [],
        durationMs: 0,
        count: 0,
        lastplayedSongId: ''
    };

    isHeartClicked: boolean = false;
    filteredPlaylist: Track[] = [];
    wishListArray: Track[] = [];
    wishList: Track[] = [];
    volumeLevel: number = 3;


    readonly TAB_ALLSONGS: string = 'all';
    readonly TAB_WISHLIST: string = 'wishlist';
    zoneId: string | null = '';
    accountId: string | null = '';

    constructor(private apiService: ApiService, private alertController: AlertController, private toastController: ToastController, private route: ActivatedRoute) { }

    ngOnInit() {
        this.zoneId = this.route.snapshot.queryParamMap.get('zoneId');
        this.accountId = this.route.snapshot.queryParamMap.get('storeID');
        this.getSongs();
        this.getNowPlayingSong();
        this.apiService.listenToSoundtrackUpdates((playingTrack: NowPlayingSong) => {
            this.removeTrackFromWishListAndLikedTracks(playingTrack)
            this.nowPlaying = playingTrack;
        });
    }

       /**
     * Removes the given track from the wishlist and the liked tracks in local storage.
     * Updates the state of the wishlist and liked tracks based on the provided playing track's ID.
     *
     * @param playingTrack - The currently playing track to be removed from wishlist and liked tracks.
     */
    removeTrackFromWishListAndLikedTracks(playingTrack: NowPlayingSong) {
        this.wishListArray = this.wishListArray.filter(track => track.id !== playingTrack.lastplayedSongId);
        const likedTracks = JSON.parse(localStorage.getItem('likedTracks') || '{}');
        if (likedTracks[playingTrack.lastplayedSongId]) {
            delete likedTracks[playingTrack.lastplayedSongId];
            localStorage.setItem('likedTracks', JSON.stringify(likedTracks));
        }
    }


    getNowPlayingSong() {
        this.apiService.getNowPlayingSong(this.zoneId || "").subscribe(
            (data: GetNowPlayingResponse) => {
                if (data.data) {
                    this.nowPlaying = data.data;
                }
            },
            (error) => {
                console.error('Error fetching now playing song:', error);
            }
        );
    }

    getSongs() {
        this.apiService.getSongs(this.zoneId ?? '').subscribe(
            async (res: GetSongsResponse) => {
                this.playlist = res.data.tracks;
                this.filteredPlaylist = [...this.playlist];
            },
            async (error) => {
                console.log(error);
            }
        );
    }

    convertMillisecondsToMinutes(milliseconds: number): string {
        const minutes = Math.floor(milliseconds / 60000);
        const seconds = Math.floor((milliseconds % 60000) / 1000);
        return `${minutes}:${seconds < 10 ? '0' : ''}${seconds}`;
    }


    getArtistNames(artists: string[]): string {
        return artists.join(', ');
    }

    onSegmentChange(event: any) {
        const selectedValue = event.detail.value;
        if (selectedValue === 'wishlist') {
            this.searchBarVisible = false;
            this.getWishList();
        } else {
            this.searchBarVisible = true;
            this.getSongs();
        }
    }

    getWishList() {
        this.apiService.getWishList(this.zoneId ?? '', this.accountId ?? '').subscribe(
            (res: GetSongsResponse) => {
                const tracks = res.data.tracks;
                if (tracks) {
                    this.wishListArray = tracks;
                    const likedTracks = JSON.parse(localStorage.getItem('likedTracks') || '{}');
                    this.wishListArray.forEach((track: Track) => {
                        track.isHeartClicked = likedTracks[track.id] || false;
                    });
                }
            },
        );
    }

 /**
     * Toggles the heart status of a track, indicating whether it is liked or not.
     * Updates the local storage with the new liked status and sends an appropriate API request
     * to either upvote or remove the upvote from the song.
     *
     * @param track - The track whose heart status is being toggled.
     */
    toggleHeart(track: Track): void {
        track.isHeartClicked = !track.isHeartClicked;
        this.updateLikedTracksInLocalStorage(track);
        const requestObject = {
            accountId: this.accountId,
            soundZone: this.zoneId,
            songId: track.id
        };
        if (track.isHeartClicked) {
            track.count = (track.count || 0) + 1;
            this.apiService.upVoteSong(requestObject).subscribe(
                async () => {
                    await this.presentToast('Song Upvoted successfully:');

                },
                (error) => {
                    console.error('Error upvoting song:', error);
                }
            );
        } else {
            track.count = Math.max((track.count || 1) - 1, 0);
            this.apiService.removeUpvote(requestObject).subscribe(
                async () => {
                    await this.presentToast('Vote removed successfully:');
                },
                (error) => {
                    console.error('Error upvoting song:', error);
                }
            );
        }
    }

    private updateLikedTracksInLocalStorage(track: Track) {
        const likedTracks = JSON.parse(localStorage.getItem('likedTracks') || '{}');
        likedTracks[track.id] = track.isHeartClicked;
        localStorage.setItem('likedTracks', JSON.stringify(likedTracks));
    }


    async addToWishList(track: Track) {
        const requestObject = {
            AccountId: this.accountId,
            SoundZone: this.zoneId,
            SongID: track.id
        };
        this.updateWishlistLocalStorage(track);
        this.apiService.addToWishList(requestObject).subscribe(
            async (res) => {
                if (res.data) {
                    await this.presentToast('✅Successfully added to the wishlist');
                }
                else {
                    await this.presentToast('✅ Successfully Upvoted!');
                }
            }
        );
    }

    /**
     * Updates the local storage for the wishlist and liked tracks based on the given track's status.
     * Toggles the heart status if the track is already liked, and adds it to the wishlist if it's not present.
     *
     * @param track - The track to be updated in local storage, containing its current liked status.
     */
    private updateWishlistLocalStorage(track: Track) {
        const addedWishlist = JSON.parse(localStorage.getItem('addedWishlist') || '[]');
        const likedTracks = JSON.parse(localStorage.getItem('likedTracks') || '{}');
        // Toggle heart status if track is already liked
        if (likedTracks[track.id]) {
            track.isHeartClicked = !track.isHeartClicked; // Toggle isHeartClicked status
        } else if (!addedWishlist.includes(track.id)) {
            // Add track to wishlist if not already present
            addedWishlist.push(track.id);
            localStorage.setItem('addedWishlist', JSON.stringify(addedWishlist));
        }
        likedTracks[track.id] = track.isHeartClicked; // Update liked status
        localStorage.setItem('likedTracks', JSON.stringify(likedTracks));
    }

    async presentToast(message: string) {
        const toast = await this.toastController.create({
            message,
            duration: 2000,
            position: 'top',
            mode: 'ios',
        });
        toast.present();
    }

    onSearchInput(event: any) {
        const searchTerm = event.target.value.toLowerCase();
        if (!searchTerm) {
            this.filteredPlaylist = [...this.playlist];
        } else {
            this.filteredPlaylist = this.playlist.filter((track: Track) => {
                return track.title.toLowerCase().includes(searchTerm);
            });
        }
    }

    async onVolumeChange(event: any) {
        const newVolume = event.detail.value;
        if (newVolume < 3) {
            await this.showVolumeAck();
            this.volumeChange(this.zoneId ?? '');
        }
    }

    async showVolumeAck() {
        const alert = await this.alertController.create({
            cssClass: 'volume-ack-alert',
            header: 'Request Considered',
            message: 'Volume will be adjusted based on voting. Wait for others to vote for reducing the volume.',
            buttons: [
                {
                    text: 'Ok',
                    role: 'confirm',
                    cssClass: 'alert-button',
                }
            ]
        });
        await alert.present();
    }

    volumeChange(zoneId: string) {
        this.apiService.volumeChange(zoneId).subscribe(
            () => {
                console.log('Volume change triggered successfully:');
            },
            () => {
                console.error('Error triggering volume change:');
            }
        );
    }

    isTrackInWishList(trackId: string): boolean {
        return this.wishListArray.some((track: { id: string; }) => track.id === trackId);
    }

}    
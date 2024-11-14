export interface Track {
    title: string;
    id: string;
    albumTitle: string;
    thumbnailUrl: string;
    artists: string[];
    durationMs: number;
    count: number;
    lastplayedSongId: string | null;
    isHeartClicked?: boolean;
}

export interface GetSongsResponse {
    success: boolean;
    data: {
        tracks: Track[];
    };
    message: string;
}


export interface NowPlayingSong {
    title: string;
    id: string;
    albumTitle: string;
    thumbnailUrl: string;
    artists: string[];
    durationMs: number;
    count: number;
    lastplayedSongId: string ;
}

export interface GetNowPlayingResponse {
    success: boolean;
    data: NowPlayingSong;
    message: string;
}
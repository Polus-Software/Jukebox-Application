import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import * as signalR from '@microsoft/signalr';

@Injectable({
    providedIn: 'root',
})
export class ApiService {
    private baseUrl = 'url';
    private hubUrl = 'url';

    

    private hubConnection: signalR.HubConnection;

    constructor(private http: HttpClient) {
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(this.hubUrl)
            .withHubProtocol(new signalR.JsonHubProtocol()) // Optional: protocol settings
            .build();
        this.hubConnection
            .start()
            .then(() => console.log("SignalR connection established"))
            .catch(err => console.error('Error while starting connection: ', err));
    }

    listenToSoundtrackUpdates(callback: (playingTrack: any) => void): void {
        this.hubConnection.on('NowPlaying', (playingTrack: any) => {
            callback(playingTrack);
        });
    }

    private handleError(error: any): Observable<never> {
        console.error('An error occurred:', error);
        return throwError(() => new Error('Something went wrong; please try again later.'));
    }

    private getHttpOptions(): { headers: HttpHeaders } {
        return {
            headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
        };
    }


    getSongs(zoneId: string): Observable<any> {
        const url = `${this.baseUrl}/getSongs`;
        const params = new HttpParams().set('zoneId', zoneId);
        return this.http.get<any>(url, { params }).pipe(catchError(this.handleError));
    }

    getWishList(zoneId: string, accountId: string): Observable<any> {
        const url = `${this.baseUrl}/getWishList`;
        const params = new HttpParams().set('accountId', accountId).set('zoneId', zoneId);
        return this.http.get<any>(url, { params }).pipe(catchError(this.handleError));
    }


    getNowPlayingSong(zoneId :string): Observable<any> {
        const url = `${this.baseUrl}/getNowPlayingSong`;
        const params = new HttpParams().set('zoneId', zoneId);
        return this.http.get<any>(url, { params }).pipe(catchError(this.handleError));
    }


    addToWishList(payload: any): Observable<any> {
        const url = `${this.baseUrl}/addToWishList`;
        return this.http.post<any>(url, payload, this.getHttpOptions()).pipe(catchError(this.handleError));
    }


    upVoteSong(payload: any): Observable<any> {
        const url = `${this.baseUrl}/upVoteSong`;
        return this.http.post<any>(url, payload, this.getHttpOptions()).pipe(catchError(this.handleError));
    }


    removeUpvote(payload: any): Observable<any> {
        const url = `${this.baseUrl}/removeUpvote`;
        return this.http.post<any>(url, payload, this.getHttpOptions()).pipe(catchError(this.handleError));
    }


    volumeChange(zoneId: string): Observable<any> {
        const url = `${this.baseUrl}/volumeChange?zoneId=${zoneId}`;
        return this.http.post<any>(url, null, this.getHttpOptions()).pipe(catchError(this.handleError));
    }

}

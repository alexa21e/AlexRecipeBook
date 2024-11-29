import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Author } from "../models/author";
import { environment } from "../../../environments/environment";

@Injectable({
    providedIn: 'root'
})

export class AuthorsService {
    baseUrl = environment.baseUrl + '/api';
    
    constructor(private http: HttpClient) {
    }

    getMostProlificAuthors(authorsNumber: number){
        let params = new HttpParams();
        params = params.append('authorsNumber', authorsNumber);
        return this.http.get<Author[]>(this.baseUrl + '/Author/mostProlific', {params});
    }
}
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FunctionUtility, OperationResult } from 'ngx-spa-utilities';
import { environment } from 'src/environments/environment';
import { KeyValue } from '@angular/common';

@Injectable({ providedIn: 'root' })
export class DivisionService {
  constructor(
    private http: HttpClient,
    private functionUtility: FunctionUtility) { }

  getProvinces(): Observable<KeyValue<string, string>[]> {
    return this.http.get<KeyValue<string, string>[]>(`${environment.apiUrl}/Division/Provinces`);
  }

  getDistricts(provinceID: string): Observable<KeyValue<string, string>[]> {
    return this.http.get<KeyValue<string, string>[]>(`${environment.apiUrl}/Division/Districts`, { params: { provinceID } });
  }

  getWards(districtID: string): Observable<KeyValue<string, string>[]> {
    return this.http.get<KeyValue<string, string>[]>(`${environment.apiUrl}/Division/Wards`, { params: { districtID } });
  }

  uploadExcel(excelFile: File): Observable<OperationResult> {
    const formData: FormData = this.functionUtility.toFormData({ excelFile });
    return this.http.post(`${environment.apiUrl}/Division`, formData);
  }
}
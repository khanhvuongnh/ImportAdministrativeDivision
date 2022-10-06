import { KeyValue } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MSG_CONST, NgxNotiflixService } from 'ngx-spa-utilities';
import { catchError, firstValueFrom, of, tap } from 'rxjs';
import { DivisionService } from './_core/services/division.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'SPA';
  file: File | null = null;
  provinceID: string = '';
  districtID: string = '';
  wardID: string = '';
  provinces: KeyValue<string, string>[] = [];
  districts: KeyValue<string, string>[] = [];
  wards: KeyValue<string, string>[] = [];

  constructor(
    private notiflixService: NgxNotiflixService,
    private divisionService: DivisionService) {
    this.notiflixService.init();
  }

  async ngOnInit(): Promise<void> {
    await this.getProvinces();
  }

  async getProvinces(): Promise<void> {
    this.provinceID = '';
    this.provinces = await firstValueFrom(this.divisionService.getProvinces());

    this.districtID = '';
    this.districts = [];

    this.wardID = ''
    this.wards = [];
  }

  async getDistricts(): Promise<void> {
    if (this.provinceID) {
      this.districtID = '';
      this.districts = await firstValueFrom(this.divisionService.getDistricts(this.provinceID));

      this.wardID = ''
      this.wards = [];
    }
  }

  async getWards(): Promise<void> {
    if (this.districtID) {
      this.wardID = '';
      this.wards = await firstValueFrom(this.divisionService.getWards(this.districtID));
    }
  }

  onFileChanged(event: any) {
    if (event.target.files && event.target.files[0]) {
      const file: File = event.target.files[0];
      if (file.name.includes('xlsx') || file.name.includes('xls')) {
        this.file = event.target.files[0];
      } else {
        this.notiflixService.error(MSG_CONST.INVALID_FILE_TYPE);
        this.file = null;
        event.target.value = '';
      }
    } else {
      this.file = null;
      event.target.value = '';
    }
  }

  async uploadExcel() {
    if (this.file) {
      this.notiflixService.showLoading();
      await firstValueFrom(this.divisionService.uploadExcel(this.file).pipe(
        tap(async res => {
          this.notiflixService.hideLoading();
          this.file = null;
          if (res.isSuccess) {
            this.notiflixService.success(MSG_CONST.UPLOADED);
            await this.getProvinces();
          } else {
            this.notiflixService.error(MSG_CONST.UPLOAD_FAILED);
          }
        }),
        catchError(() => {
          this.notiflixService.hideLoading();
          this.notiflixService.error(MSG_CONST.UNKNOWN_ERROR);
          return of();
        })
      ));
    }
  }

  async provinceChanged(): Promise<void> {
    await this.getDistricts();
  }

  async districtChanged(): Promise<void> {
    await this.getWards();
  }
}
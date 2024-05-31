import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { SharedModule } from '../shared/shared.module';
import { AboutModule } from '../about/about.module';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { ContactModule } from '../contact/contact.module';

@NgModule({
  declarations: [
    HomeComponent
  ],
  imports: [
    CommonModule,
    AboutModule,
    ContactModule,
    CarouselModule.forRoot()
  ],
  exports: [
    HomeComponent
  ]
})
export class HomeModule { }

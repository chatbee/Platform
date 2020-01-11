import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { FlexLayoutModule } from '@angular/flex-layout';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CarouselComponent } from './carousel.component';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
@NgModule({
  declarations: [CarouselComponent],
  imports: [
    CommonModule,
    FlexLayoutModule,
    MatCardModule,
    RouterModule,
    MatButtonModule,
    MatIconModule,
    PerfectScrollbarModule,
    MatMenuModule
  ],
  exports: [CarouselComponent]
})
export class CarouselModule {}

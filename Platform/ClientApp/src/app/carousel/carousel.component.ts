import { CarouselItem } from './../models/carouselItem';
import { Component, Input, ChangeDetectionStrategy } from '@angular/core';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

@Component({
  selector: 'app-carousel',
  templateUrl: './carousel.component.html',
  styleUrls: ['./carousel.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CarouselComponent {
  @Input()
  items: CarouselItem[];
  scrollConfig: PerfectScrollbarConfigInterface = {
    useBothWheelAxes: true
  };
}

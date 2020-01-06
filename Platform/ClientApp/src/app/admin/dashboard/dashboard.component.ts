import { CarouselItem } from './../../models/carouselItem';
import { Component } from '@angular/core';
import { map } from 'rxjs/operators';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {
  carouselItems: CarouselItem[] = [
    new CarouselItem(
      '',
      'test title',
      'test content',
      '',
      'https://material.angular.io/assets/img/examples/shiba1.jpg'
    ),
    new CarouselItem(
      '',
      'test title2',
      'test content2',
      '',
      'https://material.angular.io/assets/img/examples/shiba1.jpg'
    ),
    new CarouselItem(
      '',
      'test title2',
      'test content2',
      '',
      'https://material.angular.io/assets/img/examples/shiba1.jpg'
    ),
    new CarouselItem(
      '',
      'test title3',
      'test content3',
      '',
      'https://material.angular.io/assets/img/examples/shiba1.jpg'
    ),
    new CarouselItem(
      '',
      'test title4',
      'test content4',
      '',
      'https://material.angular.io/assets/img/examples/shiba1.jpg'
    ),
    new CarouselItem(
      '',
      'test title5',
      'test content5',
      '',
      'https://material.angular.io/assets/img/examples/shiba1.jpg'
    )
  ];
  /** Based on the screen size, switch from standard to one column per row */
  cards = this.breakpointObserver.observe(Breakpoints.Handset).pipe(
    map(({ matches }) => {
      if (matches) {
        return [
          { title: 'Card 1', cols: 1, rows: 1 },
          { title: 'Card 2', cols: 1, rows: 1 }
        ];
      }

      return [
        { title: 'Card 1', cols: 2, rows: 1 },
        { title: 'Card 2', cols: 1, rows: 1 }
      ];
    })
  );

  constructor(private breakpointObserver: BreakpointObserver) {}
}

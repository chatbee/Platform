import { browser, by, element } from 'protractor';

export class AppPage {
  navigateTo() {
    return browser.get(browser.baseUrl) as Promise<any>;
  }

  getTitleText() {
    return element(
      by.xpath(
        '/html/body/app-root/mat-sidenav-container/mat-sidenav-content/mat-toolbar/span'
      )
    ).getText() as Promise<string>;
  }
  getLoginTitleText() {
    return element(
      by.xpath(
        '/html/body/app-root/mat-sidenav-container/mat-sidenav-content/div/app-login/div/mat-card/mat-card-header/div/mat-card-title'
      )
    ).getText();
  }
}

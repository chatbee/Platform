export const baseUrl = {
  provide: 'BASE_URL',
  useFactory: () => {
    return 'testApi';
  },
  deps: []
};

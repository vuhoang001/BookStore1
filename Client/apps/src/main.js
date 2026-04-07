import { createApp } from 'vue';
import App from './App.vue';
import router from './router';

import Aura from '@primeuix/themes/aura';
import PrimeVue from 'primevue/config';
import ConfirmationService from 'primevue/confirmationservice';
import ToastService from 'primevue/toastservice';

import { initKeycloak } from '@/api/core/keycloak';
import '@/assets/styles.scss';
import '@/assets/tailwind.css';

const init = async () => {
    const app = createApp(App);

    app.use(router);
    app.use(PrimeVue, {
        theme: {
            preset: Aura,
            options: {
                darkModeSelector: '.app-dark'
            }
        }
    });
    app.use(ToastService);
    app.use(ConfirmationService);

    // Init Keycloak
    await initKeycloak();

    app.mount('#app');
};

init();

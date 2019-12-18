import axios, { AxiosPromise } from 'axios';
import { config } from '@/environment/config';
import { AppSettings } from '@/models/AppSettings';

export default class AppService{
    static getSettings(): AxiosPromise<any> {
        return axios.get(`${config.URL}/config`);
    }
    static saveSettings(settings: AppSettings): AxiosPromise<any> {
        return axios.post(`${config.URL}/config`, settings);
    }
    static closeApp():AxiosPromise<any>{
        return axios.put(`${config.URL}/force_exit`);
    }
}
import axios, { AxiosPromise } from 'axios';
import { config } from '@/environment/config';
import { AppSettings } from '@/models/AppSettings';

export default class AppService{
    static getSettings(): AxiosPromise<any> {
        return axios.get(`${config.HOST}/config`);
    }
    static saveSettings(settings: AppSettings): AxiosPromise<any> {
        return axios.post(`${config.HOST}/config`, settings);
    }
}
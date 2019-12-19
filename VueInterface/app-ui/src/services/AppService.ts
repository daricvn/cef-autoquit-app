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
    static getLanguage(fileName: string): AxiosPromise<any>{
        return axios.get(`${config.URL}/config/language?name=${fileName}`);
    }
    static closeApp():AxiosPromise<any>{
        return axios.put(`${config.URL}/config/force_exit`);
    }
    static openLink(link: string){
        axios.post(`${config.URL}/config/open_link`,{ link });
    }
}
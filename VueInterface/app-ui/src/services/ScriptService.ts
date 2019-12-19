import axios, { AxiosPromise } from 'axios';
import { config } from '../environment/config'

export default class ScriptService{
    static getProcesses(deep: boolean=false): AxiosPromise<any>{
        return axios.get(`${config.URL}/script/processes?deep=${deep}`);
    }

    static getProcess(pid: number): AxiosPromise<any>{
        return axios.get(`${config.URL}/script/process/details?pid=${pid}`);
    }

    static browse():AxiosPromise<any>{
        return axios.get(`${config.URL}/script/browse`);
    }
}
import axios, { AxiosPromise } from 'axios';
import { config } from '../environment/config'
import { ScriptItem } from '@/models/ScriptItem';

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

    static bringTop(pid: number): AxiosPromise<any>{
        return axios.put(`${config.URL}/script/process/top?pid=${pid}`);
    }

    static record(pid: number): AxiosPromise<any>{
        return axios.post(`${config.URL}/script/record?pid=${pid}`);
    }
    static stoprecord(): AxiosPromise<any>{
        return axios.post(`${config.URL}/script/stoprecord`);
    }
    static getcoord(pid: number): AxiosPromise<any>{
        return axios.post(`${config.URL}/script/getcoord?pid=${pid}`);
    }
    static play(pid: number[], script: ScriptItem): AxiosPromise<any>{
        return axios.post(`${config.URL}/script/play`, { pids: pid, item: script});
    }
}
export class TableData{
    selected: Array<any> | undefined;
    columns: Array<TableColumn> | undefined;
    data?: Array<any> | undefined;
}

export class TableColumn{
    name: String | undefined;
    required?: Boolean | undefined | null;
    label?: String | undefined | null;
    align?: 'left' | 'right' | 'center' | undefined;
    field: any | undefined ;
    format?: any | undefined;
    sortable?: Boolean | undefined | null;
}
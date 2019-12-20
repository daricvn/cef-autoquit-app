export class TableData<T>{
    selected: Array<any> | undefined;
    columns: Array<TableColumn> | undefined;
    data?: Array<T> | undefined;
}

export class TableColumn{
    name: String | undefined;
    required?: Boolean | undefined | null;
    label?: String | undefined | null;
    align?: 'left' | 'right' | 'center' | undefined;
    field?: any ;
    format?: any | undefined;
    sortable?: Boolean | undefined | null;
    type?: ColumnType;
    presetData?: any;
    editable?: Boolean;
    validator?: any;
    errorMessage?: String;
    min?: Number;
    max?: Number;
    visible?: Boolean;
}

export enum ColumnType{
    Text,
    Checkbox,
    List,
    File,
    Button,
    RoundButton,
    Number,
    Dynamic
}

export class ColumnButton{
    label?: String;
    action?: any;
    color?: String;
    icon?: String;
}

export class ColumnListItem{
    text?: String;
    value: String | undefined;
}
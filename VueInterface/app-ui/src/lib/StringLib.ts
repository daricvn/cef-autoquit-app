export default class StringLib{
  static format(source: string, ...args: string[]) {
    return source.replace(/{(\d+)}/g, function(match, number) { 
      return typeof args[number] != 'undefined'
        ? args[number]
        : match
      ;
    });
  };
}
const autocomplete = (options, value) => {
  let path = options.url;

  let parameter = '';
  parameter += getValue('term', value);
  parameter += getValue('record', options.resultCount);
  parameter += getValue('cityId', options.cityId);
  parameter += getValue('SourceId', options.source);

  if (parameter) {
    parameter = parameter.slice(0, -1);
    path += '?' + parameter;
  }

  return fetch(path, { method: "GET", headers: { "Accept": "application/json" } }).then(response => response.json())
}

const getValue = (key, value) => {
  if (value) {
    return key + '=' + value + '&';
  }

  return '';
}

export default autocomplete

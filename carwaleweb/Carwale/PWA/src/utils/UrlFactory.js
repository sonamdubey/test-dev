export const getModelPageUrl = (makeMaskingName, modelMaskingName) => {
  if (makeMaskingName && modelMaskingName) {
    return `/${makeMaskingName}-cars/${modelMaskingName}/`;
  }
};

export const getVersionPageUrl = (
  makeMaskingName,
  modelMaskingName,
  versionMaskingName,
  isMobile = true
) => {
  if (makeMaskingName && modelMaskingName && versionMaskingName) {
    return `${
      isMobile ? "/m" : ""
    }/${makeMaskingName}-cars/${modelMaskingName}/${versionMaskingName}/`;
  }
};

export const getCompareUrl = (data, isMobile = true) => {
  try {
    if (data) {
      let compareUrl = (isMobile ? "/m" : "") + "/comparecars/",
        i,
        queryString = "?";
      data.sort((a, b) => Number(a.modelId) > Number(b.modelId));
      for (i = data.length - 1; i >= 0; i--) {
        compareUrl += data[i].makeMaskingName + "-" + data[i].modelMaskingName;
        queryString += `c${data.length - i}=${data[i].versionId}${
          i === 0 ? "" : "&"
        }`;
        if (i !== 0) {
          compareUrl += "-vs-";
        }
      }
      return compareUrl + "/" + queryString;
    }
  } catch (e) {
    throw new Error(e);
  }
};

export function RefreshPage() {
  let url = window.location.href;
  if (url.search("price-in") >= 0) {
    url = url.split("price-in")[0];
    setTimeout(() => {
      window.location.href = url;
    }, 0);
  } else {
    setTimeout(() => {
      window.location.reload();
    }, 0);
  }
}

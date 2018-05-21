export function mapVersionsDataToDropdownList(versionPayload) {
    if (versionPayload != null && versionPayload.length > 0){
        return versionPayload.map((version, index) => {
            return {
                label : version.versionName,
                value : index,
                price : version.price,
                hostUrl : version.hostUrl,
                originalImagePath: version.originalImagePath 
            }
        });
    }
    return [];
}
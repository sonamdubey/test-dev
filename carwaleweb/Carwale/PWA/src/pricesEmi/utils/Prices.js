export const getModelData = (state) => {
    let model = state.data.filter((item) => {
        if(item.id === state.activeModelId) {
            return item
        }
    })

    if(!model.length) {
        model = state.data;
    }

    return model[0]
}

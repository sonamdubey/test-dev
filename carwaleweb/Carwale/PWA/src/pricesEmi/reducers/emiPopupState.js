let initialEMIPopupState = false
export const emiPopupState = (state = initialEMIPopupState, action) => {
    switch(action.type) {
        case 'SHOW_EMI_POPUP':
            let showState = state
            showState = !state
            return showState
        case 'HIDE_EMI_POPUP':
            let newState = state
            newState = !state
            return newState
        default:
            return state;
    }
}

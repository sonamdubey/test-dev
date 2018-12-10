export const validateManufacturingDetails = (state) => {
    if(state){
        const today = new Date()
        const currentMonth = today.getMonth() + 1; // getMonth() method returns the month (from 0 to 11) for the specified date, according to local time.
        if(state.year == state.maxYear){
            return state.month <= currentMonth
        }
        return state.year >= state.minYear && state.year < state.maxYear && state.month >=1 && state.month <= 12
    }
}

export const validateCarDetails =(state) =>{
    let isValid = false;
    let errorText = "";
    if(state){
        if(state.make.id <= 0){
            errorText = "Select make";
        }else if(state.model.id <= 0){
            errorText = "Select model";
        }else if(state.version.id <= 0){
            errorText = "Select version";
        }else{
            isValid = true;
        }
    }
    return {isValid:isValid,errorText:errorText}
}
export const validateCity = state => {
    return state.selected.cityId > 0
}

import {
  slider,
  inputBox
} from "./emiDownPayment";
import {
  slider as tenureSlider,
  inputBox as tenureInputbox
} from "./emiTenure";
import {
  slider as interestSlider,
  inputBox as interestInputbox
} from "./emiInterest";

import {
  UPDATE_DOWNPAYMENT_SLIDER_VALUE,
  UPDATE_DOWNPAYMENT_INPUT_VALUE,
  UPDATE_TENURE_SLIDER_VALUE,
  UPDATE_TENURE_INPUT_VALUE,
  UPDATE_INTEREST_SLIDER_VALUE,
  UPDATE_INTEREST_INPUT_VALUE,
  SET_DOWNPAYMENT_SNAPPOINT,
  SET_IS_EMI_VALID
} from "../actionTypes";

import {
  getSnapPointsWithInterval
} from "../../utils/rheostat/function/DiffSnapPoints";

const initialState = {
  data: [{
    id: "20",
    modelName: "Amaze",
    data: {
      vehicleData: {
        id: "1",
        makeName: "",
        modelName: "",
        versionName: ""
      },
      vehicleDownPayment: {
        inputBox: {
          value: 500000
        },
        slider: {
          max: 10000000,
          min: 9000,
          sliderTitleRight: "On-Road Price",
          snapPoints: [],
          userChange: false,
          values: [500000]
        },
        sliderInterval: []
      },
      vehicleInterest: {
        inputBox: {
          value: 2
        },
        slider: {
          max: 15,
          min: 1,
          userChange: false,
          values: [2]
        }
      },
      vehiclePie: {
        pieInterestPayable: "",
        pieTotalPrincipalAmount: "",
        pieloanAmount: "",
        pieEmiAmount: ""
      },
      vehicleTenure: {
        inputBox: {
          value: 1
        },
        slider: {
          max: 7,
          min: 1,
          userChange: false,
          values: [1]
        }
      },
      campaignTemplate: {
        htmlString: "",
        campaignType: "",
        leadClickSource: 0,
        inquirySource: 0
      },
      campaignDetails: {
        userLocation: {},
        modelId: 0,
        versionId: 0,
        campaignDealerId: 0,
        isCampaignAvailable: false
      },
      reactCampaignCta: false,
      isEmiCustomized: false,
      isEmiValid: true
    }
  }],
  activeModelId: 0,
  emiLoanamount: 0
};

export const newEmiPrices = (state = initialState, action) => {
  switch (action.type) {
    case "SET_EMI_DATA":
      return {
        ...state,
        data: action.data
      };
    case "UPDATE_EMI_MODEL":
      {
        // TODO: Update following operation to only add model only if it is not already present in the store
        let newData = state.data;
        for (let i = 0; i < action.data.length; i++) {
          const index = newData.findIndex(x => x.id == action.data[i].id);
          if (index > -1) {
            newData.splice(index, 1);
          }
          const element = action.data[i];
          newData = newData.concat(element);
        }

        return {
          ...state,
          data: newData
        };
      }
    case "SET_EMI_MODEL":
      return {
        ...state,
        activeModelId: action.id
      };
    case "SET_EMI_RESULT":
      return {
        ...state,
        emiLoanamount: action.data
      };
    case UPDATE_DOWNPAYMENT_SLIDER_VALUE:
      {
        let model = state.data.filter(item => {
          if (item.id === state.activeModelId) {
            return item;
          }
        });

        let newSlider = slider(model[0].data.vehicleDownPayment.slider, action);
        let newModelData = model.map(function (item) {
          return {
            ...item,
            data: {
              ...item.data,
              vehicleDownPayment: {
                ...item.data.vehicleDownPayment,
                slider: newSlider
              },
              isEmiCustomized: true
            }
          };
        });

        let newData = state.data.map(item => {
          if (item.id === newModelData[0].id) {
            return newModelData[0];
          } else {
            return item;
          }
        });

        return {
          ...state,
          data: newData
        };
      }

    case UPDATE_DOWNPAYMENT_INPUT_VALUE:
      {
        let model = state.data.filter(item => {
          if (item.id === state.activeModelId) {
            return item;
          }
        });

        let newInputBox = inputBox(
          model[0].data.vehicleDownPayment.inputBox,
          action
        );
        let newModelData = model.map(function (item) {
          return {
            ...item,
            data: {
              ...item.data,
              vehicleDownPayment: {
                ...item.data.vehicleDownPayment,
                inputBox: newInputBox
              },
              isEmiCustomized: true
            }
          };
        });

        let newData = state.data.map(item => {
          if (item.id === newModelData[0].id) {
            return newModelData[0];
          } else {
            return item;
          }
        });

        return {
          ...state,
          data: newData
        };
      }

    case UPDATE_TENURE_SLIDER_VALUE:
      {
        let model = state.data.filter(item => {
          if (item.id === state.activeModelId) {
            return item;
          }
        });

        let newSlider = tenureSlider(model[0].data.vehicleTenure.slider, action);
        let newModelData = model.map(function (item) {
          return {
            ...item,
            data: {
              ...item.data,
              vehicleTenure: {
                ...item.data.vehicleTenure,
                slider: newSlider
              },
              isEmiCustomized: true
            }
          };
        });

        let newData = state.data.map(item => {
          if (item.id === newModelData[0].id) {
            return newModelData[0];
          } else {
            return item;
          }
        });

        return {
          ...state,
          data: newData
        };
      }

    case UPDATE_TENURE_INPUT_VALUE:
      {
        let model = state.data.filter(item => {
          if (item.id === state.activeModelId) {
            return item;
          }
        });

        let newInputBox = tenureInputbox(
          model[0].data.vehicleTenure.inputBox,
          action
        );
        let newModelData = model.map(function (item) {
          return {
            ...item,
            data: {
              ...item.data,
              vehicleTenure: {
                ...item.data.vehicleTenure,
                inputBox: newInputBox
              },
              isEmiCustomized: true
            }
          };
        });

        let newData = state.data.map(item => {
          if (item.id === newModelData[0].id) {
            return newModelData[0];
          } else {
            return item;
          }
        });

        return {
          ...state,
          data: newData
        };
      }
    case UPDATE_INTEREST_SLIDER_VALUE:
      {
        let model = state.data.filter(item => {
          if (item.id === state.activeModelId) {
            return item;
          }
        });

        let newSlider = interestSlider(
          model[0].data.vehicleInterest.slider,
          action
        );
        let newModelData = model.map(function (item) {
          return {
            ...item,
            data: {
              ...item.data,
              vehicleInterest: {
                ...item.data.vehicleInterest,
                slider: newSlider
              },
              isEmiCustomized: true
            }
          };
        });

        let newData = state.data.map(item => {
          if (item.id === newModelData[0].id) {
            return newModelData[0];
          } else {
            return item;
          }
        });

        return {
          ...state,
          data: newData
        };
      }

    case UPDATE_INTEREST_INPUT_VALUE:
      {
        let model = state.data.filter(item => {
          if (item.id === state.activeModelId) {
            return item;
          }
        });

        let newInputBox = interestInputbox(
          model[0].data.vehicleInterest.inputBox,
          action
        );
        let newModelData = model.map(function (item) {
          return {
            ...item,
            data: {
              ...item.data,
              vehicleInterest: {
                ...item.data.vehicleInterest,
                inputBox: newInputBox
              },
              isEmiCustomized: true
            }
          };
        });

        let newData = state.data.map(item => {
          if (item.id === newModelData[0].id) {
            return newModelData[0];
          } else {
            return item;
          }
        });

        return {
          ...state,
          data: newData
        };
      }

    case SET_DOWNPAYMENT_SNAPPOINT:
      const model = state.data.filter(item => {
        if (item.id === state.activeModelId) {
          return item;
        }
      });
      const downpayment = model[0].data.vehicleDownPayment;
      let snapPoints = getSnapPointsWithInterval(downpayment.sliderInterval);

      // restore min downpayment value
      snapPoints[0] = downpayment.slider.min;

      const newSlider = slider(downpayment.slider, {
        type: SET_DOWNPAYMENT_SNAPPOINT,
        snapPoints
      });

      const newModelData = model.map(function (item) {
        return {
          ...item,
          data: {
            ...item.data,
            vehicleDownPayment: {
              ...item.data.vehicleDownPayment,
              slider: newSlider
            }
          }
        };
      });

      const newData = state.data.map(item => {
        if (item.id === newModelData[0].id) {
          return newModelData[0];
        }

        return item;
      });

      return {
        ...state,
        data: newData
      };

    case SET_IS_EMI_VALID:
      let modifiedCurrentModel = state.data.filter(item => {
        return item.id === state.activeModelId;
      }).map(item => {
        return {
          ...item,
          data: {
            ...item.data,
            isEmiValid: action.value
          }
        };
      });

      if (typeof modifiedCurrentModel[0] !== "undefined") {
        const modifiedData = state.data.map(item => {
          if (item.id === modifiedCurrentModel[0].id) {
            return modifiedCurrentModel[0];
          }
          return item;
        });
        return {
          ...state,
          data: modifiedData
        }
      } else {
        return {
          ...state
        }
      }

    default:
      return state;
  }
};

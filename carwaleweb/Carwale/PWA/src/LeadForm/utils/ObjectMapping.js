import { sourceType } from "../Enum/SourceType";

export function recoMappingApiToStore(recoData) {
  return {
    campaign: {
      id: recoData.campaign.id,
      name: recoData.campaign.contactName
    },
    carDetail: {
      makeId: recoData.carData.carMake.makeId,
      makeName: recoData.carData.carMake.makeName,
      modelId: recoData.carData.carModel.modelId,
      modelName: recoData.carData.carModel.modelName,
      hostUrl: recoData.carData.carImageBase.hostUrl,
      originalImgPath: recoData.carData.carImageBase.originalImgPath
    },
    priceOverview: {
      price: recoData.carPricesOverview.price,
      priceStatus: recoData.carPricesOverview.priceStatus
    }
  };
}

export function MLAMappingApiToStore(MLAData) {
  return {
    campaign: {
      id: MLAData.campaign.id
    },
    seller: {
      id: MLAData.dealerDetails.id,
      name: MLAData.dealerDetails.name,
      area: MLAData.dealerDetails.area,
      distance: MLAData.dealerDetails.distance
    }
  };
}

export function leadMappingBuyerStoreToApi(state, leadData) {
  const { leadClickSource } = state;
  const { others } = state.NC.leadForm;

  let othersData = {};
  if (Object.keys(others).length > 0) {
    Object.keys(others).map(othersKey => {
      othersData[othersKey] = others[othersKey].value;
    });
  }

  return {
    UserInfo: {
      Name: leadData.buyerInfo.name,
      Email: leadData.buyerInfo.email,
      Mobile: leadData.buyerInfo.mobile
    },
    UserLocation: {
      CityId: leadData.cityId,
      AreaId: state.location.areaId
    },
    LeadSource: {
      PlatformId: leadClickSource.page.platform.id,
      ApplicationId: leadClickSource.page.applicationId,
      SourceType: sourceType.PRIMARY,
      PageId: leadClickSource.page.page.id,
      PropertyId: leadClickSource.propId,
      IsCitySet: leadClickSource.isCitySet
    },
    CarInquiry: [
      {
        CarDetail: {
          ModelId: leadData.modelDetail.modelId,
          VersionId: leadData.modelDetail.versionId
        },
        Seller: {
          CampaignId: state.NC.campaign.campaign.id,
          AssignedDealerId: leadData.assignedDealerId
        }
      }
    ],
    EncryptedLeadId: "",
    Others: {
      TestDrive: leadData.testDriveChecked ? 1 : 0,
      ...othersData
    }
  };
}

export function leadMappingCampaignStoreToApi(state, selectedMLA, modelDetail) {
  let CarInquiry = [];
  const { NC, location, leadClickSource } = state;
  const { others } = state.NC.leadForm;

  let othersData = mapOthers(others);

  let MLAList = NC.leadForm.MLASellers.list;
  selectedMLA.forEach(element => {
    CarInquiry.push({
      CarDetail: {
        ModelId: modelDetail.modelId,
        VersionId: modelDetail.versionId
      },
      Seller: {
        CampaignId: MLAList[element].campaign.id,
        AssignedDealerId: MLAList[element].seller.id
      }
    });
  });

  return {
    UserInfo: {
      Name: NC.leadForm.buyerInfo.name,
      Email: NC.leadForm.buyerInfo.email,
      Mobile: NC.leadForm.buyerInfo.mobile
    },
    UserLocation: {
      CityId: location.cityId,
      AreaId: location.areaId
    },
    LeadSource: {
      PlatformId: leadClickSource.page.platform.id,
      ApplicationId: leadClickSource.page.applicationId,
      SourceType: sourceType.MLA,
      PageId: leadClickSource.page.page.id,
      PropertyId: leadClickSource.propId,
      IsCitySet: leadClickSource.isCitySet
    },
    CarInquiry,
    EncryptedLeadId: "",
    Others: {
      TurboMla: state.NC.campaign.campaign.isTurboMla ? 1 : 0,
      ...othersData
    }
  };
}

export function leadMappingCarDetailStoreToApi(state, selectedReco) {
  let CarInquiry = [];
  const { NC, location, leadClickSource } = state;
  const { others } = state.NC.leadForm;

  let othersData = mapOthers(others);

  let recoList = NC.leadForm.recommendation.list;
  selectedReco.forEach(element => {
    let selectedRecodata = recoList[element];
    CarInquiry.push({
      CarDetail: {
        MakeId: selectedRecodata.carDetail.makeId,
        ModelId: selectedRecodata.carDetail.modelId
      },
      Seller: {
        CampaignId: selectedRecodata.campaign.id
      }
    });
  });

  return {
    UserInfo: {
      Name: NC.leadForm.buyerInfo.name,
      Email: NC.leadForm.buyerInfo.email,
      Mobile: NC.leadForm.buyerInfo.mobile
    },
    UserLocation: {
      CityId: location.cityId,
      AreaId: location.areaId
    },
    LeadSource: {
      PlatformId: leadClickSource.page.platform.id,
      ApplicationId: leadClickSource.page.applicationId,
      SourceType: sourceType.RECO,
      PageId: leadClickSource.page.page.id,
      PropertyId: leadClickSource.propId,
      IsCitySet: leadClickSource.isCitySet
    },
    CarInquiry,
    EncryptedLeadId: "",
    Others: { ...othersData }
  };
}

export function leadMappingEmailStoreToApi(state, email) {
  const { NC, location, leadClickSource } = state;
  const leadIdItems = NC.leadForm.leadId;
  return {
    UserInfo: {
      Name: NC.leadForm.buyerInfo.name,
      Email: email,
      Mobile: NC.leadForm.buyerInfo.mobile
    },
    UserLocation: {
      CityId: location.cityId,
      AreaId: location.areaId
    },
    LeadSource: {
      PlatformId: leadClickSource.page.platform.id,
      ApplicationId: leadClickSource.page.applicationId,
      PageId: leadClickSource.page.page.id,
      PropertyId: leadClickSource.propId,
      IsCitySet: leadClickSource.isCitySet,
      SourceType: sourceType.PRIMARY
    },
    CarInquiry: [
      {
        CarDetail: {
          ModelId: NC.campaign.featuredCarData.modelId,
          VersionId: 0 //get versionId from store
        },
        Seller: {
          CampaignId: state.NC.campaign.campaign.id,
          AssignedDealerId: -1
        }
      }
    ],
    EncryptedLeadId: leadIdItems.join(),
    Others: { EmailLead: 1 }
  };
}

export function mapBhriguLogs(logList) {
  let mappedLogs = [];
  let bhriguLog;
  let interactionId;
  let logTimeStamp;
  let stringifiedLog;
  logList.forEach(log => {
    interactionId = log.interactionId;
    logTimeStamp = log.timestamp;
    stringifiedLog = JSON.stringify(log);
    bhriguLog = {
      cat: "leadform",
      act: "newcars",
      lbl: "logData=" + stringifiedLog + "| interactionId=" + interactionId,
      ts: logTimeStamp
    };
    mappedLogs.push(bhriguLog);
  });

  return mappedLogs;
}

function mapOthers(others) {
  let othersData = {};
  if (Object.keys(others).length > 0) {
    Object.keys(others).map(othersKey => {
      if (others[othersKey].type == sourceType.ALL) {
        othersData[othersKey] = others[othersKey].value;
      }
    });
  }
  return othersData;
}

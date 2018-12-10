IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NewCRM_ProcessData_FetchDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NewCRM_ProcessData_FetchDetails]
GO

	--Name of SP/Function                    : NewCRM_ProcessData_FetchDetails
--Applications using SP                  : NewCRM(common function)
--Modules using the SP                   : ProcessData.cs
--Technical department                   : Database
--Summary                                : fetching all data based on flcId(CRM_VerificationLog's id)
--Author                                 : AMIT Kumar 26-Nov-2013
--Modification history                   : 1. Vaibhav K 24 June 2014 - Added left join to get data for Tata leads & few parameters added
--Modification history					 : 2. Yuga Hatolkar 30th October - Added parameter isHDFCFinance
--Modified								 : Vaibhav K 4-1-2015 - ModelCode, VersionCode set as ModelName, VersionName
--Modofied								 : Vaibhav K 20-04-2016 added ModelCode, VersionCode - CRM_TATAVersionMapping, DealerDivId - CRM_TataCityDealers
CREATE PROCEDURE [dbo].[NewCRM_ProcessData_FetchDetails]
@flcDataId			NUMERIC(18,0),
@cbdId				NUMERIC(18,0) OUT,
@versionId			NUMERIC(18,0) OUT,
@makeId				NUMERIC(18,0) OUT,
@modelId			NUMERIC(18,0) OUT,
@carName			VARCHAR(200) OUT,
@carMakeName		VARCHAR(200) OUT,
@cityId				NUMERIC(18,0) OUT,
@dealerId			NUMERIC(18,0) OUT,
@dealershipName		VARCHAR(200) OUT,
@comments			VARCHAR(MAX) OUT,
@leadId				NUMERIC(18,0) OUT,
@ldStatgeId			VARCHAR(20) OUT,
@sourceId			VARCHAR(200) OUT,
@ldGroupType		VARCHAR(20) OUT,
@ldGroupCategory	VARCHAR(20) OUT,
@eagerness			VARCHAR(40) OUT,
@buyTime			VARCHAR(20) OUT,
@oprUserId			NUMERIC(18,0) OUT,
@cName				VARCHAR(200) OUT,
@cMobile			VARCHAR(30) OUT,
@cEmail				VARCHAR(200) OUT,
@customerId			NUMERIC(18,0) OUT,
@TDRequestDate		DATETIME OUT,
@isTD				BIT	OUT,
@isPQ				BIT	OUT,
@isConCall			BIT	OUT,
@isPD				BIT	OUT,
@carModel			VARCHAR(200) OUT,
@carVersion			VARCHAR(200) OUT,
@City				VARCHAR(200) OUT,
@PurchaseMode		VARCHAR(200) OUT,
@PurchaseOnNameType	VARCHAR(200) OUT,
@PurchaseOnName		VARCHAR(200) OUT,
@PurchaseContact	VARCHAR(200) OUT,
@buyingSpan			VARCHAR(200) OUT,
@dealerType			INT OUT,
@isFinance			BIT = 0	OUT,
@isOffer			BIT = 0	OUT,
@isUrgent			BIT = 0	OUT,
@Pincode			VARCHAR(10) = NULL OUT,
@StateCode			VARCHAR(2) = NULL OUT,
@ModelCode			VARCHAR(50) = NULL OUT,
@VersionCode		VARCHAR(50) = NULL OUT,
@DealerDivId		VARCHAR(50) = NULL OUT,
@TataCityName		VARCHAR(50) = NULL OUT,
@isHDFCFinance		BIT = 0 OUT

AS
	BEGIN
		SELECT TOP 1 @cbdId  =  CVL.CBDId,	@versionId  =  CVL.VersionId,	@dealerId  =  CVL.DealerId,		@comments  =  CVL.Comments,		@leadId  =  CVL.LeadId,		@makeId  =  MMV.MakeId,
				@isTD  =  CVL.IsTDRequired,		@isPQ  =  CVL.IsPQRequired,		@isConCall  =  CVL.IsConCall,	@isPD  =  CVL.IsPDRequired,		@modelId  =  MMV.ModelId,	@carName  =  mmv.Car ,	 
				@carMakeName  =  MMV.Make,	@buyTime  =  CVOL.PurchaseTime,	  @eagerness  =  CVOL.Eagerness,	@dealershipName  =  ND.Name ,	@ldStatgeId  =  CL.LeadStageId,
				@ldGroupType  =  CAF.GroupType,		@ldGroupCategory  =  CLS.CategoryId,	@oprUserId  =  CVL.UpdatedBy,	@sourceId  =  (CASE CLS.CategoryId WHEN 3 THEN CLS.SourceId ELSE 1 END), @cityId  =  CC.CityId,
				@customerId  =  CC.ID,	 @cEmail  =  CC.Email,	@cMobile  =  CC.Mobile,		@cName  =  (CC.FirstName + ' ' + CC.LastName) , @TDRequestDate = CVL.TDDate,
				@carModel  =  MMV.Model,	@carVersion  =  MMV.Version,@City=c.Name,@PurchaseMode=CVOL.PurchaseMode,@PurchaseOnNameType = CVOL.PurchaseOnNameType ,@PurchaseOnName = CVOL.PurchaseOnName,@buyingSpan=CVOL.BuyingSpan,
				@dealerType = ISNULL(ND.DealerType, 100), @isFinance = CVL.IsFinance, @isOffer = CVL.IsOffer, @isUrgent = CVL.IsUrgent,
				@Pincode = CI.DefaultPinCode, --TPC.Pincode, --@Pincode = C.DefaultPinCode, 
				@StateCode = ST.StateCode, --@DealerDivId = ND.DealerCode, 
				@TataCityName = CI.Name,-- TCMP.CityName,
				@ModelCode = ISNULL(TVP.ModelCode, (
													SELECT TOP 1 ModelCode FROM CRM_TATAVersionMapping WITH(NOLOCK) WHERE ModelId = MMV.ModelId
													)
									),
                @VersionCode = ISNULL(TVP.VersionCode,(
														SELECT TOP 1 VersionCode FROM CRM_TATAVersionMapping  WITH(NOLOCK)WHERE ModelId = MMV.ModelId
														)
										),
				@isHDFCFinance = CVOL.IsHDFC,
				@DealerDivId =
				CASE 
					WHEN MMV.ModelId = 229 OR MMV.ModelId = 841 --Nano and Nano GenX
						THEN 
							( 
								SELECT Top 1  CTD.DealerDivisionId
								FROM CRM_TataCityDealers CTD WITH (NOLOCK)
								WHERE CTD.IsActive = 1
							AND 
							(
							CTD.CWCityId = CI.Id AND CTD.PplRowId = '1-2XIV6FL' -- Deales mapped with Nano model
							OR 
							CTD.CWCityId = CI.Id)) 
					WHEN MMV.ModelId = 585 --Zest
						THEN 
							(SELECT Top 1  CTD.DealerDivisionId
							FROM CRM_TataCityDealers CTD WITH (NOLOCK)
							WHERE CTD.IsActive = 1
							AND 
							(
							CTD.CWCityId = CI.Id AND CTD.PplRowId = '1-A54A6T7' -- Deales mapped with Zest model
							OR 
							CTD.CWCityId = CI.Id)) 
					WHEN MMV.ModelId = 852 --Tiago
						THEN 
							(SELECT Top 1  CTD.DealerDivisionId
							FROM CRM_TataCityDealers CTD WITH (NOLOCK)
							WHERE CTD.IsActive = 1
							AND 
							(
							CTD.CWCityId = CI.Id AND CTD.PplRowId = '1-DR4I0XM' -- Deales mapped with Tiago model
							OR 
							CTD.CWCityId = CI.Id)) 
					ELSE ''				
				END				
		FROM CRM_VerificationLog CVL WITH(NOLOCK)
			INNER JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CVL.CBDId = CBD.ID
			INNER JOIN Cities C WITH(NOLOCK) ON CBD.CityId=C.Id			
			INNER JOIN CRM.vwMMV MMV WITH(NOLOCK) ON MMV.VersionId = CVL.VersionId
			INNER JOIN CRM_VerificationOthersLog CVOL WITH(NOLOCK) ON CVOL.LeadId = CVL.LeadId
			INNER JOIN CRM_Leads CL WITH(NOLOCK) ON CL.ID = CVL.LeadId
			INNER JOIN CRM_ADM_FLCGroups CAF WITH(NOLOCK) ON CAF.Id=CL.GroupId
			INNER JOIN CRM_LeadSource CLS WITH(NOLOCK) ON CL.Id=CLS.LeadId
			INNER JOIN NCS_Dealers ND WITH(NOLOCK) ON ND.ID = CVL.DealerId
			INNER JOIN CRM_Customers CC WITH(NOLOCK) ON CL.CNS_CustId=CC.Id 
			INNER JOIN Cities AS CI  WITH(NOLOCK) ON ND.CityId=CI.Id
			INNER JOIN States ST WITH(NOLOCK) ON CI.StateId = ST.ID
			LEFT JOIN CRM_TATAVersionMapping TVP WITH(NOLOCK) ON MMV.VersionId = TVP.VersionId
			--LEFT JOIN CRM_TATACityMapping TCMP WITH(NOLOCK) ON ND.CityId = TCMP.CityId --C.Id = TCMP.CityId
			---LEFT JOIN CRM_TATAPincodes TPC WITH(NOLOCK) ON ND.ID = TPC.DealerId
		WHERE CVL.Id= @flcDataId
		--print '@@cbdId ' + @cbdId				
		--print '@@versionId ' + @versionId			
		--print '@@makeId ' + @makeId				
		--print '@@modelId ' + @modelId
		--print '@@carName ' + @carName			
		--print '@@carMakeName ' + @carMakeName		
		--print '@@cityId ' + @cityId	
		--print '@@dealerId ' + @dealerId
		--print '@@dealershipName ' + @dealershipName		
		--print '@@comments ' + @comments	
		--print '@@leadId ' + @leadId	
		--print '@@ldStatgeId ' + @ldStatgeId	
		--print '@@sourceId ' + @sourceId		
		--print '@@ldGroupType ' + @ldGroupType
		--print '@@ldGroupCategory ' + @ldGroupCategory
		--print '@@eagerness ' + @eagerness	
		--print '@@buyTime ' + @buyTime
		--print '@@oprUserId ' + @oprUserId
		--print '@@cName ' + @cName
		--print '@@cMobile ' + @cMobile
		--print '@@cEmail ' + @cEmail
		--print '@@customerId ' + @customerId
		--print @isTD
		--print @isPQ
		--print @isConCall
		--print @isPD			
	END

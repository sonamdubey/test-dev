IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_FetchCarTestDriveData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_FetchCarTestDriveData]
GO

	
--Summary: Fetch Details of Test Drive
--Author:
--Modifier: Dilip V. 22-Mar-2012 (Added TD Direct Completed)

CREATE PROCEDURE [dbo].[CRM_FetchCarTestDriveData]

	@CarBasicDataId			Numeric,
	@CarTestDriveId			Numeric OutPut,
	@LeadId					Numeric OutPut,
	@TDLocationId			SmallInt OutPut,
	@TDLocationType			VarChar(100) OutPut,
	@TDLocation				VarChar(500) OutPut,
	@TDStatusId				SmallInt OutPut,
	@TDStatus				VarChar(100) OutPut,
	@DealerId				Numeric OutPut,
	@DealerName				VarChar(200) OutPut,
	@Comments				VarChar(1000) OutPut,
	@UpdatedById			Numeric OutPut,
	@UpdatedByName			VarChar(100) OutPut,
	@ContactPerson			VarChar(50) OUTPUT,
	@Contact				VarChar(50) OUTPUT,

	@TDRequestDate			DateTime OutPut,
	@TDCompletedDate		DateTime OutPut,
	@CreatedOn				DateTime OutPut,
	@UpdatedOn				DateTime OutPut
				
 AS
	DECLARE @ISTDCompleted	AS BIT,
			@ISTDNP			AS BIT,
			@IsTDDirect		AS BIT
	
BEGIN

	SELECT	
		@CarTestDriveId		= CTD.Id,
		@LeadId				= CBD.LeadId,
		@TDLocationId		= CTD.TDLocationType,
		@TDLocationType		= TDL.Name,
		@TDLocation			= CASE CTD.TDLocationType WHEN 1 THEN COD.ResAddress WHEN 2 THEN COD.OfficeAddress WHEN 3 THEN ND.Address END,
		@TDStatus			= TDS.Name,
		@DealerId			= CTD.DealerId,
		@DealerName			= ND.Name,
		@Comments			= CTD.Comments,
		@UpdatedById		= CTD.UpdatedBy,
		@UpdatedByName		= OU.UserName,
		@ContactPerson		= CTD.ContactPerson,	
		@Contact			= CTD.Contact,
		@CreatedOn			= CTD.CreatedOn,
		@UpdatedOn			= CTD.UpdatedOn

	FROM (((((((CRM_CarTestDriveData AS CTD LEFT JOIN CRM_CarBasicData AS CBD ON CTD.CarBasicDataId = CBD.Id)
			LEFT JOIN OprUsers AS OU ON CTD.UpdatedBy = OU.Id)
			LEFT JOIN NCS_Dealers AS ND ON CTD.DealerId = ND.Id)
			LEFT JOIN CRM_EventTypes AS TDS ON CTD.TDStatusId = TDS.Id)
			LEFT JOIN CRM_TDLocationTypes AS TDL ON CTD.TDLocationType =  TDL.Id)
			LEFT JOIN CRM_Leads AS CL ON CL.Id = CBD.LeadId)
			LEFT JOIN CRM_CustomerOtherDetails AS COD ON CL.CNS_CustId = COD.CRM_CustomerId)
			

	WHERE CTD.CarBasicDataId = @CarBasicDataId
	
	SELECT Top 1 @TDRequestDate = TDRequestDate, @TDCompletedDate = TDCompleteDate,@IsTDDirect = IsTDDirect, 
	@ISTDCompleted = IsTDCompleted, @ISTDNP = ISTDNotPossible
	FROM CRM_CarTDLog WHERE CBDId = @CarBasicDataId ORDER BY ID DESC
	
	IF @@ROWCOUNT <> 0
		BEGIN
			IF @ISTDCompleted = 1
				SET @TDStatusId = 14
			ELSE IF @ISTDNP = 1
				SET @TDStatusId = 15
			ELSE IF @IsTDDirect = 1
				SET @TDStatusId = 64
			ELSE
				SET @TDStatusId = 7
		END
	
END



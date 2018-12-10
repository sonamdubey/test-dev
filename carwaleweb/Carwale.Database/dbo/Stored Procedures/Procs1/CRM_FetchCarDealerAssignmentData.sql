IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_FetchCarDealerAssignmentData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_FetchCarDealerAssignmentData]
GO

	
--Summary  : Get Details of Dealers to SMS
--Author   : 
--Modifier : Dilip V. 12-March-2012(Contact number from NCS_Dealers table instead of CRM_CarDealerAssignment)

CREATE PROCEDURE [dbo].[CRM_FetchCarDealerAssignmentData]

	@CarBasicDataId			Numeric,
	@CDAId					Numeric OUTPUT,
	@DealerId				Numeric OUTPUT,
	@DealerName				VarChar(200) OUTPUT,
	@StatusId				SmallInt OUTPUT,
	@Status					VarChar(100) OUTPUT,
	@CreatedById			Numeric OUTPUT,
	@CreatedByName			VarChar(100) OUTPUT,
	@UpdatedById			Numeric OUTPUT,
	@UpdatedByName			VarChar(100) OUTPUT,
	@Comments				VarChar(1000) OUTPUT,
	@ContactPerson			VarChar(50) OUTPUT,
	@Contact				VarChar(50) OUTPUT,
	@LostName				VarChar(100) OUTPUT,
	@ReasonLost				VarChar(100) OUTPUT,

	@CreatedOn				DateTime OUTPUT,
	@UpdatedOn				DateTime OUTPUT,
	@LostDate				DateTime = NULL OUTPUT,
	@DealershipStatus		BIT = NULL OUTPUT,
	@IsFollowDone			BIT = NULL OUTPUT
				
 AS
BEGIN

	SELECT	
		@CDAId				= CDA.Id,
		@DealerId			= CDA.DealerId,
		@DealerName			= ND.Name,
		@StatusId			= CDA.Status,
		@Status				= ET.Name,
		@CreatedById		= CDA.CreatedBy,
		@CreatedByName		= OU.UserName,
		@UpdatedById		= CDA.UpdatedBy,
		@UpdatedByName		= OU1.UserName,
		@Comments			= CDA.Comments,
		@ContactPerson		= ND.ContactPerson,	
		@Contact			= ND.Mobile,
		@CreatedOn			= CDA.CreatedOn,
		@UpdatedOn			= CDA.UpdatedOn,
		@LostDate			= CDA.LostDate,
		@LostName			= CDA.LostName,
		@ReasonLost			= CDA.ReasonLost,
		@DealershipStatus	= CDA.DealershipStatus,	
		@IsFollowDone		= CDA.IsFollowDone			

	FROM ((((CRM_CarDealerAssignment AS CDA WITH(NOLOCK) 
			LEFT JOIN OprUsers AS OU WITH(NOLOCK) ON CDA.CreatedBy = OU.Id)
			LEFT JOIN OprUsers AS OU1 WITH(NOLOCK) ON CDA.UpdatedBy = OU1.Id)
			LEFT JOIN NCS_Dealers AS ND WITH(NOLOCK) ON CDA.DealerId = ND.Id)
			LEFT JOIN CRM_EventTypes AS ET WITH(NOLOCK) ON CDA.Status = ET.Id)

	WHERE CDA.CBDId = @CarBasicDataId
END





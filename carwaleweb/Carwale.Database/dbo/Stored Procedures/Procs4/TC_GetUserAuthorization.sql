IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetUserAuthorization]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetUserAuthorization]
GO

	-- =============================================
-- Author:		<Nilesh Utture>
-- Create date: <22nd April, 2013>
-- Description:	<This procedure will give the info about the user access to the lead followup page>
-- EXEC TC_GetUserAuthorization 3351,5,226
-- Modified by Umesh on 12-06-2013 
-- Modified By: Nilesh Utture on 18th June, 2013 Added New code to get all the parent users
--Modified By : Ashwini Dhamamkar on Sept 30,2016 (Added condition for advantage lead type)
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetUserAuthorization]
	-- Add the parameters for the stored procedure here
	@UserId BIGINT,
	@BranchId BIGINT,
	@LeadId BIGINT,
	@IsBuyerAuthorized BIT = 0 OUTPUT,
	@IsSellerAuthorized BIT = 0 OUTPUT,
	@IsNewAuthorized BIT = 0 OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

DECLARE @TC_UserId BIGINT
DECLARE @RoleId SMALLINT
--DECLARE @TC_ReportingTo BIGINT
DECLARE @BuyOwnerId INT
DECLARE @SellOwnerId INT
DECLARE @NewOwnerId INT
DECLARE @ReportingToList TABLE(Id INT) -- Modified By: Nilesh Utture on 18th June, 2013

SELECT @BuyOwnerId = TC_UserId FROM TC_InquiriesLead WITH (NOLOCK) WHERE BranchId = @BranchId AND TC_LeadId = @LeadId AND TC_LeadInquiryTypeId = 1;
SELECT @SellOwnerId = TC_UserId FROM TC_InquiriesLead WITH (NOLOCK) WHERE BranchId = @BranchId AND TC_LeadId = @LeadId AND TC_LeadInquiryTypeId = 2;
SELECT @NewOwnerId = TC_UserId FROM TC_InquiriesLead WITH (NOLOCK) WHERE BranchId = @BranchId AND TC_LeadId = @LeadId AND TC_LeadInquiryTypeId IN (3,5); --Modified By : Ashwini Dhamamkar on Sept 30,2016

    -- Insert statements for procedure here
	SELECT TOP(1) @TC_UserId = U.Id, 
				  @RoleId = R.RoleId 
					FROM		TC_Users U WITH (NOLOCK)
					INNER JOIN  TC_UsersRole R WITH (NOLOCK)
					ON		 U.Id = @UserId 
					AND		 R.UserId = @UserId
					WHERE	 R.RoleId IN (1,7,12) 
					ORDER BY R.RoleId
	
	IF @NewOwnerId IS NOT NULL AND @NewOwnerId = @UserId 
		BEGIN
			SET @IsNewAuthorized = 1
		END	
	IF @BuyOwnerId IS NOT NULL AND @BuyOwnerId = @UserId
		BEGIN
			SET @IsBuyerAuthorized = 1
		END	
	IF @SellOwnerId IS NOT NULL AND @SellOwnerId = @UserId
		BEGIN
			SET @IsSellerAuthorized = 1
		END	

	IF @TC_UserId IS NOT NULL 
	BEGIN 
		IF @RoleId = 1 OR @RoleId = 12
		BEGIN
			SET @IsBuyerAuthorized = 1
			SET @IsSellerAuthorized = 1
			SET @IsNewAuthorized = 1
		END
		ELSE IF @RoleId = 7
		BEGIN 
			DECLARE @Emp HIERARCHYID
			DECLARE @lvl SMALLINT
			IF @BuyOwnerId IS NOT NULL AND @BuyOwnerId <> @UserId
			BEGIN 
				INSERT INTO @ReportingToList  EXEC TC_GetALLParent @BuyOwnerId -- Modified By: Nilesh Utture on 18th June, 2013
				IF ((SELECT COUNT(Id) FROM @ReportingToList WHERE Id = @TC_UserId) = 1)
				BEGIN
					SET @IsBuyerAuthorized = 1
				END
			END
			
			IF @SellOwnerId IS NOT NULL AND @SellOwnerId <> @UserId
			BEGIN 
				INSERT INTO @ReportingToList  EXEC TC_GetALLParent @SellOwnerId -- Modified By: Nilesh Utture on 18th June, 2013
				IF ((SELECT COUNT(Id) FROM @ReportingToList WHERE Id = @TC_UserId) = 1)
				BEGIN
					SET @IsSellerAuthorized = 1
				END
			END
			
			IF @NewOwnerId IS NOT NULL AND @NewOwnerId <> @UserId 
			BEGIN 
				INSERT INTO @ReportingToList  EXEC TC_GetALLParent @NewOwnerId -- Modified By: Nilesh Utture on 18th June, 2013
				IF ((SELECT COUNT(Id) FROM @ReportingToList WHERE Id = @TC_UserId) = 1)
				BEGIN
					SET @IsNewAuthorized = 1
				END
			END
		END
	END
END


IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_SaveUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_SaveUpdate]
GO

	
/*
	This procedure created on 12 Jan 2010 by Sentil
	for update and save for ESM	
	Updated By : Vinay Kumar  Prajapati 
	Purpose : Add or update Data
*/
-- =============================================
-- Author	:	Vinay Kumar  Prajapati (12 Jan 2010)
-- Description	:	This procedure created on 12 Jan 2010 by Sentil
--					for update and save for ESM
-- Modifier	:	Sachin Bharti(20th Aug 2015)
-- Modifier :	  Amit Yadav(1st Oct 2015)
-- Description :  Condition to avoid duplicate data and added parameter @RetValue 
-- =============================================
CREATE Procedure [dbo].[ESM_SaveUpdate] 
(
	@ID AS NUMERIC(18,0),	
	@OrgName AS VARCHAR(50),
	@type AS SMALLINT,
	@IsActive AS BIT,
	@UpdatedOn AS DATETIME,
	@UpdatedBy AS NUMERIC(18,0),
	@AccountManager AS VARCHAR(100),--Added By Ajay Singh()
	@RetValue AS NUMERIC(18,0) OUT	--Added By Amit Yadav
)
AS

BEGIN
	SET @RetValue= -1
	IF(@ID = -1)
		BEGIN
			--Avoid Duplicate Entry
			SELECT * FROM ESM_OrganizationName AS EON WITH(NOLOCK) WHERE EON.OrgName=@OrgName
			IF @@ROWCOUNT = 0
				BEGIN
					INSERT INTO ESM_organizationName (OrgName, type, IsActive, UpdatedOn, UpdatedBy,AccountManager)
											   VALUES(@OrgName, @type, @IsActive, @UpdatedOn, @UpdatedBy,@AccountManager)

	
					SET @RetValue = SCOPE_IDENTITY()
				END
		END
	ELSE
		BEGIN
			--Avoid Duplicate Entry
			SELECT * FROM ESM_OrganizationName AS EON WITH(NOLOCK) WHERE EON.OrgName=@OrgName AND EON.id<>@ID
			IF @@ROWCOUNT = 0
				BEGIN
					UPDATE ESM_organizationName SET 
					OrgName = @OrgName, type = @type, IsActive = @IsActive, UpdatedOn = @UpdatedOn, UpdatedBy = @UpdatedBy, AccountManager=@AccountManager
					WHERE id = @ID
					SET @RetValue = @ID
				END
				
		END	 

END

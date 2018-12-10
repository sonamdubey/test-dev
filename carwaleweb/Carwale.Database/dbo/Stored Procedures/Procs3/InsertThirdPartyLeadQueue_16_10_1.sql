IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertThirdPartyLeadQueue_16_10_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertThirdPartyLeadQueue_16_10_1]
GO

	-- =============================================
-- Author:		Sanjay
-- Create date: 23/08/2016
-- Description:	Inserts and update record into ThirdPartyLeadQueue
-- Modified By : Added ThirdPartyLeadId condition in update statement
-- Modified By : Sanjay Soni on 01/10/2016 Removed IsPushToThirdParty condition in update statement
-- =============================================
CREATE PROCEDURE [dbo].[InsertThirdPartyLeadQueue_16_10_1]
	-- Add the parameters for the stored procedure here
	@PQId NUMERIC(18, 0)
	,@ModelName VARCHAR(30)
	,@ModelId INT
	,@CityId INT
	,@CustomerName VARCHAR(100)
	,@Email VARCHAR(100)
	,@Mobile VARCHAR(20)
	,@ThirdPartyLeadId NUMERIC(18, 0)
	,@ThirdPartyLeadQueueId  INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @MakeId NUMERIC,
		@City VARCHAR(30)
	
	SELECT @MakeId = CM.CarMakeId
	FROM CarModels CM WITH (NOLOCK)
	WHERE CM.ID = @ModelId
	
	IF (
			@City = ''
			OR @City IS NULL
			)
		SELECT @City = NAME
		FROM Cities WITH (NOLOCK)
		WHERE ID = @CityId

	IF(EXISTS (SELECT ThirdPartyLeadId FROM ThirdPartyLeadQueue WITH (NOLOCK) where PQID = @PQId))
	BEGIN
		UPDATE ThirdPartyLeadQueue 
			SET CustomerName = @CustomerName, Email = @Email, Mobile = @Mobile, CityId = @CityId, City = @City, @ThirdPartyLeadQueueId = ThirdPartyLeadId
		WHERE PQID = @PQId AND TPLeadSettingId = @ThirdPartyLeadId
	END
	ELSE
	BEGIN
		INSERT INTO ThirdPartyLeadQueue (
			PQId
			,ModelName
			,MakeId
			,ModelId
			,TPLeadSettingId
			,CityId
			,City
			,CustomerName
			,Email
			,Mobile
			,PushStatus
			)
		VALUES (
			@PQId
			,@ModelName
			,@MakeId
			,@ModelId
			,@ThirdPartyLeadId
			,@CityId
			,@City
			,@CustomerName
			,@Email
			,@Mobile
			,- 1
			)
		
		SET @ThirdPartyLeadQueueId = SCOPE_IDENTITY()
	END
END


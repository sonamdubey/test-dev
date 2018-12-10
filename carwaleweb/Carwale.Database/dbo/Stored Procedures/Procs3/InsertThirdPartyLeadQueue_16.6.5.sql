IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertThirdPartyLeadQueue_16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertThirdPartyLeadQueue_16]
GO

	-- =============================================
-- Author:		Vinayak
-- Create date: 1/03/2016
-- Description:	Inserts a record into ThirdPartyLeadQueue
-- =============================================
create PROCEDURE [dbo].[InsertThirdPartyLeadQueue_16.6.5]
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

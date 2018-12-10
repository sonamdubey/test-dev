IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_PushCheck]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_PushCheck]
GO

	-- =============================================
-- Author:		Reshma Shetty
-- Create date: 10/4/2013
-- Description:	<Returns a bit which signifies whether the lead has to be pushed into the CRM or not> 
--DECLARE @Bit BIT EXEC CRM_PushCheck 467,1,@Bit OUTPUT SELECT @Bit
--Modified By : Raghu on 13-06-2013 for adding parameter @IsUserResearching
-- =============================================
CREATE PROCEDURE [dbo].[CRM_PushCheck]
	-- Add the parameters for the stored procedure here
		@ModelId INT,
		@CityId INT,
		@IsUserResearching int, -- user buying prefrences
		@Bit BIT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    SET @Bit = 0
    DECLARE @MakeId INT
	
	SELECT @MakeId = CM.CarMakeId
    FROM CarModels CM WITH(NOLOCK) WHERE Id = @ModelId
    
    -- Insert statements for procedure here
	IF @IsUserResearching = 1 -- If user is researching
		BEGIN
			IF EXISTS(SELECT TOP 1 Id FROM CRM_ADM_Queues WITH(NOLOCK)
			WHERE IsActive = 1 AND ID IN (
						SELECT QueueId FROM CRM_ADM_QueueRuleParams WITH(NOLOCK)
						WHERE (
								(MakeId =@MakeId OR MakeId =-1 )
								AND (ModelId =@ModelId OR ModelId =-1)
								AND (CityId = @CityId OR CityId = -1)
								AND (IsResearch = 1) -- Connside both both buying and researching 
								)
							)AND AcceptNewLead=1)
			    --IF @@ROWCOUNT = 0
				SET @Bit = 1
			ELSE
				SET @Bit=0
		END
	--ELSE IF @IsUserResearching = 0 -- User Is Buying
	--	BEGIN
	--		IF EXISTS(SELECT TOP 1 Id FROM CRM_ADM_Queues
	--		WHERE IsActive = 1 AND ID IN (
	--					SELECT QueueId FROM CRM_ADM_QueueRuleParams
	--					WHERE (
	--							(MakeId =@MakeId OR MakeId =-1 )
	--							AND (ModelId =@ModelId OR ModelId =-1)
	--							AND (CityId = @CityId OR CityId = -1)
	--							AND ( IsResearch = 1 OR IsResearch = 0) 
	--							)
	--						)AND AcceptNewLead=1)
			
	--			--IF @@ROWCOUNT = 0
	--			SET @Bit = 1
	--		ELSE
	--			SET @Bit=0	
		
	--	END
	ELSE 
		BEGIN 
			IF EXISTS(SELECT TOP 1 Id FROM CRM_ADM_Queues
			WHERE IsActive = 1 AND ID IN (
						SELECT QueueId FROM CRM_ADM_QueueRuleParams
						WHERE (
								(MakeId =@MakeId OR MakeId =-1 )
								AND (ModelId =@ModelId OR ModelId =-1)
								AND (CityId = @CityId OR CityId = -1)
								)
							)AND AcceptNewLead=1)
			

				SET @Bit = 1
			ELSE
			BEGIN
				SET @Bit=0
END
END
END



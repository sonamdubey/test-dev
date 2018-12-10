IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRMPushCheck]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRMPushCheck]
GO

	-- =============================================
-- Author:		Reshma Shetty
-- Create date: 10/4/2013
-- Description:	<Returns a bit which signifies whether the lead has to be pushed into the CRM or not> 
--DECLARE @Bit BIT EXEC CRM_PushCheck 467,1,@Bit OUTPUT SELECT @Bit
--Modified By : Raghu on 13-06-2013 for adding parameter @IsUserResearching
-- Modified by : Raghu on 30-12-2013 added WITH(NOLOCK) Condition
-- =============================================
CREATE procedure [dbo].[CRMPushCheck]
	-- Add the parameters for the stored procedure here
		@VersionId INT,
		@CityId INT,
		@IsUserResearching BIT, -- user buying prefrences
		@Bit BIT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @MakeId INT,@ModelId INT

	SELECT @MakeId = MakeId,@ModelId = ModelId FROM vwMMV WHERE VersionId = @VersionId
    
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
								AND (IsResearch = 1) -- Consider both both buying and researching 
								)
							)AND AcceptNewLead=1)
			    --IF @@ROWCOUNT = 0
				SET @Bit = 1
			ELSE
				SET @Bit=0
		END
	ELSE 
		BEGIN 
			IF EXISTS(SELECT TOP 1 Id FROM CRM_ADM_Queues WITH(NOLOCK)
			WHERE IsActive = 1 AND ID IN (
						SELECT QueueId FROM CRM_ADM_QueueRuleParams WITH(NOLOCK)
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



/****** Object:  StoredProcedure [dbo].[MSMQCheck]    Script Date: 12/30/2013 2:08:37 PM ******/
SET ANSI_NULLS ON


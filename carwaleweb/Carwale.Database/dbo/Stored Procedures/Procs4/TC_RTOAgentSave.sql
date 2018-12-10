IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_RTOAgentSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_RTOAgentSave]
GO

	
-- =============================================
-- ModifiedBy:		Binumon George
-- Create date: 08-11-2011
-- Description:	Added ModifiedBy parameter and modified date.
-- =============================================  
-- =============================================
-- Author:		Binumon George
-- Create date: 11th October 2011
-- Description:	This procedure is used to add update RTO Agent
-- =============================================
CREATE PROCEDURE [dbo].[TC_RTOAgentSave]
(
@TC_RTOAgent_Id INT =NULL,
@TC_RTO_Id INT,
@AgentName VARCHAR(50),
@DealerId NUMERIC,
@ModifiedBy INT
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF(@TC_RTOAgent_Id IS NULL) --Insering Dealer's RTO Agent
	BEGIN
	
		--IF NOT EXISTS(SELECT Agent.TC_RTOAgent_Id  FROM TC_RTOAgent Agent INNER JOIN TC_RTO RTO ON RTO.TC_RTO_Id=Agent.TC_RTO_Id WHERE RTO.DealerId=@DealerId AND Agent.AgentName=@AgentName)
		IF NOT EXISTS(SELECT TC_RTOAgent_Id  FROM TC_RTOAgent  WHERE TC_RTO_Id =@TC_RTO_Id AND AgentName=@AgentName AND IsActive=1)
		BEGIN
			INSERT TC_RTOAgent(AgentName,TC_RTO_Id, ModifiedBy) VALUES(@AgentName,@TC_RTO_Id,@ModifiedBy)
		END
		ELSE
		BEGIN
			RETURN -2 -- Means Duplicate record is already exists in DB
		END		
	END
	ELSE --  Updating  RTO Agent
	BEGIN
		IF NOT EXISTS(SELECT TC_RTOAgent_Id  FROM TC_RTOAgent WHERE TC_RTO_Id =@TC_RTO_Id AND  AgentName=@AgentName AND TC_RTOAgent_Id<>@TC_RTOAgent_Id AND IsActive=1)
		BEGIN
			UPDATE TC_RTOAgent SET AgentName=@AgentName, TC_RTO_Id=@TC_RTO_Id, ModifiedBy=@ModifiedBy, ModifiedDate=GETDATE() WHERE TC_RTOAgent_Id=@TC_RTOAgent_Id
		END
		ELSE
		BEGIN
			RETURN -2 -- Means Duplicate record is already exists in DB
		END		
	END 
		SELECT Agent.TC_RTOAgent_Id ,Agent.AgentName,RTO.RTOName,RTO.TC_RTO_Id FROM TC_RTOAgent Agent
		INNER JOIN TC_RTO RTO ON RTO.TC_RTO_Id=Agent.TC_RTO_Id
		WHERE RTO.DealerId=@DealerId AND Agent.IsActive=1
END


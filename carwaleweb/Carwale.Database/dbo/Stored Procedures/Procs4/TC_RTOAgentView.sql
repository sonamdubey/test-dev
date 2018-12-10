IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_RTOAgentView]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_RTOAgentView]
GO

	 CREATE PROCEDURE [dbo].[TC_RTOAgentView]
(
@DealerId NUMERIC
)
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;
SELECT Agent.TC_RTOAgent_Id ,Agent.AgentName,RTO.RTOName,RTO.TC_RTO_Id FROM TC_RTOAgent Agent
INNER JOIN TC_RTO RTO ON RTO.TC_RTO_Id=Agent.TC_RTO_Id
WHERE RTO.DealerId=@DealerId AND Agent.IsActive=1

SELECT TC_RTO_Id,RTOName FROM TC_RTO WHERE DealerId=@DealerId AND IsActive=1

END

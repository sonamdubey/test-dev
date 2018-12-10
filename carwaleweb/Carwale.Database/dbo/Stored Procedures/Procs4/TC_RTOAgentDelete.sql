IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_RTOAgentDelete]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_RTOAgentDelete]
GO

	-- =============================================
-- Author:		Binumon George
-- Create date: 30th Nov 2011
-- Description:	Added status parameter
-- =============================================
-- Author:		Binumon George
-- Create date: 12 th October 2011
-- Description:	This procedure will be used to Delete RTO Agent
-- =============================================
CREATE PROCEDURE [dbo].[TC_RTOAgentDelete]
(
@DealerId NUMERIC,
@TC_RTOAgent_Id INT,
@Status INT OUTPUT
)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @Status=0
	IF NOT EXISTS(SELECT Top 1 * FROM TC_BookingRTO WHERE TC_RTOAgent_Id=@TC_RTOAgent_Id AND IsActive=1)
		BEGIN
		-- here we checking dealer with agent from TC_RTO table
			IF EXISTS(SELECT Agent.TC_RTOAgent_Id  FROM TC_RTOAgent Agent INNER JOIN TC_RTO RTO ON RTO.TC_RTO_Id=Agent.TC_RTO_Id WHERE RTO.DealerId=@DealerId AND Agent.TC_RTOAgent_Id=@TC_RTOAgent_Id)
				BEGIN
					UPDATE TC_RTOAgent SET IsActive = 0 WHERE TC_RTOAgent_Id=@TC_RTOAgent_Id
					SET @Status=1
				END
		END
		ELSE
		BEGIN
			SET @Status=2 --record refrence with TC_BookingRTO table. so cant delete
		END
END

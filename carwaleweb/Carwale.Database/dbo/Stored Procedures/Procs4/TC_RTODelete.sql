IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_RTODelete]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_RTODelete]
GO

	-- =============================================
-- Author:		Binumon George
-- Create date: 30th Nov 2011
-- Description:	Added status parameter
-- =============================================
-- Author:		Binumon George
-- Create date: 11th October 2011
-- Description:	This procedure will be used to Delete RTO
-- Modified By: Tejashree Patil on 6 Sept 2012 on 4 pm 
-- Desciption:  Added condition in select clause agent.IsActive=1
-- =============================================
CREATE PROCEDURE [dbo].[TC_RTODelete]
(
@DealerId NUMERIC,
@TC_RTO_Id INT,
@Status INT OUTPUT
)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @Status=0

		-- Here checking booking.TC_RTO_Id and agent.TC_RTO_Id NULL
		-- If Null its converting into 0
		IF (SELECT TOP 1 CASE  WHEN ISNULL(booking.TC_RTO_Id,0) =0
			AND ISNULL(agent.TC_RTO_Id,0)=0 THEN 0 ELSE 1 END status
			FROM TC_RTO rto 
			LEFT JOIN  TC_BookingRTO  booking ON rto.TC_RTO_Id=booking.TC_RTO_Id
			LEFT JOIN  TC_RTOAgent agent ON rto.TC_RTO_Id = agent.TC_RTO_Id AND agent.IsActive=1
			WHERE rto.TC_RTO_Id=@TC_RTO_Id) = 0
			
			BEGIN
				-- if above result =0 then deleting records
			  UPDATE TC_RTO SET IsActive=0 WHERE TC_RTO_Id=@TC_RTO_Id AND DealerId=@DealerId
			  SET @Status=1
			END
		ELSE
			BEGIN
				SET @Status=2 --record refrence with TC_BookingRTO and TC_RTOAgent tables. so cant delete
			END
END



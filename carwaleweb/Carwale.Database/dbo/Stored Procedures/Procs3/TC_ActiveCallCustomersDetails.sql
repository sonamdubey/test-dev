IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ActiveCallCustomersDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ActiveCallCustomersDetails]
GO

	-- ================================================================================================
-- Author: Upendra Kumar 
-- Create date: 26 nov,2015
-- Description:	this will return all active Calls Customer Mobile no and name .
-- EXEC TC_ActiveCallCustomersDetails 243 
-- ================================================================================================
CREATE PROCEDURE [dbo].[TC_ActiveCallCustomersDetails] @UserId INT
	--@BranchId    INT,
AS
BEGIN
	SELECT DISTINCT TCD.CustomerName AS NAME
		,TCD.Mobile AS Mobile
	FROM TC_ActiveCalls TAC WITH (NOLOCK)
	INNER JOIN TC_CustomerDetails TCD WITH (NOLOCK) ON TAC.TC_LeadId = TCD.ActiveLeadId --.Id
	WHERE TAC.TC_UsersId = @UserId
		AND ISNULL(TCD.IsActive, 0) = 1
		AND ISNULL(TCD.Isfake, 0) = 0 --AND TCD.BranchId = @BranchId
END



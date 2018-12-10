IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CheckCwAccessAlert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CheckCwAccessAlert]
GO

	
-- =============================================
-- Author:		Surendra Chouksey
-- Create date: 6th Dec,2011
-- Description:	This procedure will Check whether CW user has requested dealer to login in Trading cars
-- =============================================
CREATE PROCEDURE [dbo].[TC_CheckCwAccessAlert]
(
@BranchId NUMERIC
)	
AS
BEGIN		
	SELECT COUNT(TC_Alerts_id) FROM TC_Alerts WHERE BranchId=@BranchId AND Status IS NULL	
END

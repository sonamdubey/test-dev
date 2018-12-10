IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetBlockedNumbers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetBlockedNumbers]
GO

	
-- =============================================
-- Author:		Vinayak					
-- Create date: 5/8/2016
-- Description:	get blocked numbers
-- =============================================
create PROCEDURE [dbo].[GetBlockedNumbers]
AS
BEGIN
select Mobile from BlockedNumbers WITH(NOLOCK) 	
END


IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateResponseTime_ZipDial]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateResponseTime_ZipDial]
GO

	-- =============================================
-- Author:		Akansha
-- Create date: 19.06.2013
-- Description:	Update TokenResponseTime For particular client id Exec UpdateResponseTime_ZipDial 'DC-D45545-27','4050d98bf0f5d73425fb09c60349dff0dc6e5bcb'
--Modified by akansha on 07-08-2013
-- =============================================
CREATE Procedure [dbo].[UpdateResponseTime_ZipDial]
@ClientId varchar(50),
@TransactionToken varchar(50)
As
Update ZipDial_Transactions set TransactionToken=@TransactionToken, TokenResponseTime=GETDATE() where ClientTransactionId=@ClientId
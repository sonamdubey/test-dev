IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BhartiAxa_UpdateLeadURL]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BhartiAxa_UpdateLeadURL]
GO

	-- =============================================
-- Author:		Akansha
-- Create date: 28.02.2014	
-- Description:	Update the lead URL given by bharti axa
-- =============================================
CREATE PROCEDURE [dbo].[BhartiAxa_UpdateLeadURL]
@RecordId int,
@URL varchar(500)

AS
BEGIN
Update BhartiAxa_Leads set BhartiAxaQuoteID=@URL
where id=@RecordId
END



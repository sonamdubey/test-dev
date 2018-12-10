IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BhartiAxa_UpdateLeadURLForSerco]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BhartiAxa_UpdateLeadURLForSerco]
GO

	-- =============================================
-- Author:		<Author,,Ashish verma>
-- Create date: <Create Date,30/04/2014,>
-- Description:	<Description,or saving response from serco,>
-- =============================================
CREATE PROCEDURE [dbo].[BhartiAxa_UpdateLeadURLForSerco]
	-- Add the parameters for the stored procedure here
@RecordId int,
@SercoResponse varchar(500)
AS
BEGIN
Update BhartiAxa_Leads set
SercoResponse=@SercoResponse where id=@RecordId
END

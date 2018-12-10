IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CV_GetCUICode]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CV_GetCUICode]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 27/2/2012 2.20 PM
-- Description:	Procedure to return cuiCode to the buyer for validation purpose
-- Parameters : email and mobile number of the buyer are the input parameters to the procedure.
--				If sms is sent to the buyer get latest cuiCode for the buyer and return it.
-- =============================================
CREATE PROCEDURE [dbo].[CV_GetCUICode]
	-- Add the parameters for the stored procedure here
	@email AS VARCHAR(50)=NULL,
	@mobile AS VARCHAR(50)=NULL,
	@cuiCode AS varchar(50) OUTPUT
AS
BEGIN
	-- Get cuicode 
    IF ISNULL(@email,'') <> '' AND  ISNULL(@mobile,'') <> ''
		BEGIN
			SET @cuiCode = ISNULL((SELECT TOP 1 pl.CUICode FROM CV_PendingList AS pl WHERE pl.Email=@email and pl.Mobile=@mobile ORDER BY pl.EntryDateTime DESC),'')
			--print @cuiCode
		END	
	ELSE
		BEGIN
			SET @cuiCode = ''
		END
END

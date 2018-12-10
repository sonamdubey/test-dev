IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetTransmissionType]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[GetTransmissionType]
GO

	-- =============================================
-- Author:		Umesh Ojha
-- Create date: 10-April-2012
-- Description:	Return transmission type of model
-- =============================================
CREATE FUNCTION [dbo].[GetTransmissionType]
(	
	-- Add the parameters for the function here
	@TransmissionValue INT
)
RETURNS Varchar(10)
AS
BEGIN
	DECLARE @TransmissionText Varchar(10)	
	SET @TransmissionText = (CASE @TransmissionValue
		WHEN  2 THEN 'Manual'
		WHEN  1 THEN 'Automatic'		
	ELSE '' END)
	RETURN @TransmissionText
END

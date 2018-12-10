IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetAllMakes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetAllMakes]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: 24-Sep-2014
-- Description:	Get all makes for RSA form.
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetAllMakes]
AS  
BEGIN  
 SET NOCOUNT ON;  
 
	SELECT Id AS Value, Name AS Text 
	FROM CarMakes WITH(NOLOCK)
	WHERE IsDeleted = 0 
	AND Futuristic = 0
	AND IsDeleted = 0
	ORDER BY Name 
END 


/****** Object:  StoredProcedure [dbo].[TC_GetRSAPackages]    Script Date: 9/24/2014 6:00:37 PM ******/
SET ANSI_NULLS ON

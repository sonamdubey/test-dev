IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[ValidateEMailMobileAndName]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[ValidateEMailMobileAndName]
GO

	
-- Author:   Naresh Palaiya
-- Create date: 20/05/2014
-- Description:	Verifiy the mobile number, name and email id of the person and update the corresponding flags in the table (pq_clientinfo)

CREATE PROCEDURE [CRM].[ValidateEMailMobileAndName]
        @PQID bigint,
	@mobiNum varchar(15),
	@name  varchar(50),
	@email varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

     IF EXISTS (select TOP 1 c.Number from [CRM].[MobilePatterns] as c  WITH (NOLOCK) where Number = substring(@mobiNum,1,4))
	   begin
	      update pq_clientinfo set ls_mobile_flag = 1 where pqid = @PQID
	   end    
	  else 
	   begin 
	    update pq_clientinfo set ls_mobile_flag = 0 where pqid = @PQID
	   end
	   
	     IF NOT EXISTS (select TOP 1 N.ID from [CRM].[NameKeyWord] as n WITH (NOLOCK) where CHARINDEX (n.keyword,@name)<>0)
      begin
	       IF ((@name  like '%a%' or @name  like '%e%' or @name  like '%i%' or @name  like '%o%' or @name  like '%u%'))  --IF name contains the vowel than only it's valid 

	      	update pq_clientinfo set ls_name_flag = 1 where pqid = @PQID
	   end 
	   else
	    begin
		    update pq_clientinfo set ls_name_flag = 0 where pqid = @PQID
		end
	   
	   IF EXISTS (select TOP 1 nm.ID from [CRM].[NameKeyWord] as nm  WITH (NOLOCK) where CHARINDEX (nm.keyword,@email)<>0)
	   begin
	      update pq_clientinfo set ls_email_flag = 0 where pqid = @PQID
	   end    
	   else
	    begin
		 update pq_clientinfo set ls_email_flag = 1 where pqid = @PQID
		end
END

IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwImagedetails_06082012' AND
     DROP VIEW dbo.vwImagedetails_06082012
GO

	






CREATE VIEW [dbo].[vwImagedetails_06082012]
as
select ID,'Con_RoadTestPages' as Table_Name,'/rt/'+cast(RTId as varchar(100))+'/'  as DirPath,MainImgPath as Filename,HostURL from Con_RoadTestPages  where IsReplicated=0  
union ALL
select ID,'CarPhotos' as Table_Name,DirectoryPath as DirPath,ISNULL(ImageUrlFull,'')+'$'+ISNULL(ImageUrlThumb,'')+'$'+ISNULL(ImageUrlThumbSmall,'') as Filename,HostURL from CarPhotos  where (IsReplicated=0 OR IsReplicated is null) AND IsActive = 1 AND HostURL Is Not Null
union ALL
select ID,'TC_CarPhotos' as Table_Name,DirectoryPath as DirPath,ISNULL(ImageUrlFull,'')+'$'+ISNULL(ImageUrlThumb,'')+'$'+ISNULL(ImageUrlThumbSmall,'') as Filename,HostURL from TC_CarPhotos  where IsReplicated=0 AND IsActive = 1
union ALL
select ID,'CarBodyStyles' as Table_Name ,'/CarBodyStyles/' as DirPath,ImageUrl as Filename,HostURL from CarBodyStyles  where IsReplicated=0  
union ALL
select ID,'CarGalleryPhotos' as Table_Name ,'/cg/'+cast(modelId as varchar(100)) as DirPath,PhotoURL as Filename,HostURL from CarGalleryPhotos  where IsReplicated=0  
union ALL
select ID,'CarMakes' as Table_Name ,'/CarMakes/' as DirPath,LogoUrl as Filename,HostURL from CarMakes  where IsReplicated=0  
union ALL
select ID,'CarModels' as Table_Name ,'/cars/' as DirPath,ISNULL(SmallPic,'')+'$'+ISNULL(LargePic,'') as Filename,HostURL from CarModels  where IsReplicated=0  and LEN(ISNULL(SmallPic+'$'+LargePic,''))>1
union ALL
select ID,'CarVersions' as Table_Name ,'/cars/' as DirPath,ISNULL(SmallPic,'')+'$'+ISNULL(LargePic,'') as Filename,HostURL from CarVersions  where IsReplicated=0  and LEN(ISNULL(SmallPic+'$'+LargePic,''))>1
union ALL
--select ID,'Con_EditCms_Images' as Table_Name ,'/ec/'+cast(BasicId as varchar(100))+'/' as DirPath,cast(ID as varchar(100)) as Filename,HostURL from Con_EditCms_Images  where IsReplicated=0  
--union ALL
select ID,'Con_RoadTest' as Table_Name ,'/rt/'+cast(Id as varchar(100))+'/' as DirPath,MainImgPath as Filename,HostURL from Con_RoadTest  where IsReplicated=0  
union ALL
select ID,'Con_RoadTestAlbum' as Table_Name ,'/rt/'+CAST(RTId as varchar(10))+'/' as DirPath,ISNULL(CAST(Id as varchar(10)),'') as Filename,HostURL from Con_RoadTestAlbum  where IsReplicated=0 
union all
select ID,'Con_FeaturedListings' as Table_Name ,'/featuredcar/' as DirPath,ISNULL(CAST(Id as varchar(10)),'') as Filename,HostURL from Con_FeaturedListings  where IsReplicated=0 
union all
 select u.ID,'UP_Photos' as Table_Name ,'/c/p/'+CAST(a.userid as varchar(10))+'/' as DirPath,(case Size500 when 1 then u.Name+'_500.jpg' else '' end)+'$'+(case Size800 when 1 then u.Name+'_800.jpg' else '' end)+'$'+(case Size1024 when 1 then u.Name+'_1024.jpg' else '' end) as Filename ,u.HostURL from UP_Photos as U  join UP_Albums as a on u.AlbumId=a.ID  where u.IsReplicated=0 
union all
 select ID,'Wallpapers' as Table_Name ,(case Eight when 1 then '/wp/800/' else '' end)+'$'+(case Thousand when 1 then '/wp/1000/' else '' end) as DirPath,RandomString+'.jpg' as Filename,HostURL from wallpapers  where IsReplicated=0 
 union all
 select TC_Bug_Id as ID,'TC_BugFeedback' as Table_Name , '/tc/bugs/' as DirPath,REPLACE(filepath,'/tc/bugs/','') as Filename,HostURL from TC_BugFeedback  where IsReplicated=0 
 union all
 select Id as ID,'Acc_Items' as Table_Name , '/accessories/items/' as DirPath,CAST(id as varchar(50))+'.jpg' as Filename,HostURL from Acc_Items  where IsReplicated=0 
union all
select Id as ID,'Acc_ItemsAdditionalImages' as Table_Name , '/accessories/items/' as DirPath,AddionalImagesLocation as Filename,HostURL from Acc_ItemsAdditionalImages  where IsReplicated=0 
union all
select Authorid as ID,'Con_EditCms_Author' as Table_Name , '/ec/authorImg/'+ cast(Authorid as varchar(50)) as DirPath,isnull(PicName+'.jpg','') as Filename,HostURL from Con_EditCms_Author  where IsReplicated=0 
union all
select Id as ID,'UserProfile' as Table_Name , '/c/up/a/$/c/up/r/' as DirPath,AvtarPhoto+'$'+RealPhoto as Filename,HostURL from UserProfile  where IsReplicated=0 and LEN(ISNULL(AvtarPhoto+'$'+RealPhoto,''))>1
union ALL 
--select ID,'Con_EditCms_Basic' as Table_Name ,'/ec/'+cast(Id as varchar(100))+'/' as DirPath,cast(ID as varchar(100)) as Filename,HostURL from Con_EditCms_Basic  where IsReplicated=0  
--union all
select ID,'ShowRoomPhotos' as Table_Name ,
	case ImageCategory when 3 then '/dealer/img/HeaderImages/' else '/dealer/img/' end as DirPath,
	case ImageCategory when 3 then ISNULL(Thumbnail,'') else ISNULL(Thumbnail,'')+'$'+ISNULL(LargeImage,'') end as Filename,
	HostURL from ShowRoomPhotos  where IsReplicated=0
union all
select ID,'Con_EditCms_Images' as Table_Name ,
case HasCustomImg when 1 then '/ec/'+cast(BasicId as varchar(100))+'/img/l/$/ec/'+cast(BasicId as varchar(100))+'/img/t/$/ec/'+cast(BasicId as varchar(100))+'/img/ol/$/ec/'+cast(BasicId as varchar(100))+'/img/c/'
when 0 then '/ec/'+cast(BasicId as varchar(100))+'/img/l/$/ec/'+cast(BasicId as varchar(100))+'/img/t/$/ec/'+cast(BasicId as varchar(100))+'/img/ol/'
end as DirPath,
cast(id as varchar(100)) As Filename,
HostURL from Con_EditCms_Images where IsReplicated=0
union ALL
select ID,'Con_EditCms_Basic' as Table_Name ,'/ec/'+cast(Id as varchar(100))+'/' as DirPath,
case HasCustomImg when 1 then cast(ID as varchar(100))+'.jpg$'+cast(ID as varchar(100))+'_m.jpg$'+cast(ID as varchar(100))+'_l.jpg$'+cast(ID as varchar(100))+'_ol.jpg$'+cast(ID as varchar(100))+'_c.jpg' 
when 0 then cast(ID as varchar(100))+'.jpg$'+cast(ID as varchar(100))+'_m.jpg$'+cast(ID as varchar(100))+'_l.jpg$'+cast(ID as varchar(100))+'_ol.jpg' 
end as Filename,HostURL from Con_EditCms_Basic where IsReplicated=0 







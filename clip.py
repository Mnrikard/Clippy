"""For those of you interested in history, this was the "second" version of clippy but the first full version, so still version 1. 
This is where clippy got its name: clip.py
The first version was written in perl.  I have since lost the perl file.  If I ever find it again, I'll put it here for posterity.
"""

__author__ = "Matthew Rikard"
__version__ = "1.5"
__date__ = "$Date: 2008/05/02$"
__license__ = "Python"
import re
import sys
import win32clipboard as w 
gt = ">"
lt = "<"

def remFromArr(arr,itm=""):
	try:
		arr.remove(itm)
		remFromArr(arr,itm)
	except:
		blnk = ""

def GetText():
	"Gets Text from the clipboard (win32)\nReturns CF_TEXT string"
	w.OpenClipboard() 
	d=w.GetClipboardData(w.CF_TEXT) 
	w.CloseClipboard() 
	return d.decode("utf8","replace") #we have to convert from a bytes object to a string

def SetText(aString):
	"Sets Text to the clipboard (win32)\nReturns None"
	w.OpenClipboard()
	w.EmptyClipboard()
	w.SetClipboardText(aString) 
	w.CloseClipboard()
	
def tabRight(srows,tabcount=0):
	rows = srows.split("\n")
	for i in range(len(rows)):
		if(re.search("^\s*//",rows[i])):#if its a commented line, skip it
			continue
		rows[i] = re.sub("^\s+","",re.sub("\s+$","",rows[i]))
		if(len(rows[i])>0 and rows[i][0] == "}"):
			tabcount = tabcount-1
		for t in range(tabcount):
			rows[i] = "\t"+rows[i]
		if(len(rows[i])>0 and rows[i][-1] == "{"):
			tabcount = tabcount+1
	return "\n".join(rows)

def tabRightVB(srows,tabcount=0):
	tplus = re.compile("^(while|for|if|class|function|sub|do|private\s+function|public\s+function|private\s+class|public\s+class|private\s+sub|public\s+sub|property|public\s+property|private\s+property)[\s]",2)
	negateIf = re.compile("^if(.)+then\s(.)+$",2)
	tminus = re.compile("^(wend|end|until|loop)[\s]?",2)
	
	srows = srows.replace("\r","")
	rows = srows.split("\n")
	for i in range(len(rows)):
		rows[i] = re.sub("^\s+","",re.sub("\s+$","",rows[i]))
		if(tminus.search(rows[i]) or rows[i].lower()=="else"):
			tabcount = tabcount-1
		for t in range(tabcount):
			rows[i] = "\t"+rows[i]
		if tplus.search(rows[i])  or rows[i].lower()=="else":
			if negateIf.search(rows[i]) == None:
				tabcount = tabcount+1
	return "\n".join(rows)

def formatJS(txt):
	txt = txt.replace("\n","").replace("\r","").replace("\t","")
	tabs = 0
	newtext = ""
	return tabRight(txt.replace("{","{\n").replace("}","\n}\n").replace(";",";\n"))
	for i in range(len(txt)):
		if txt[i]=="{":
			tabs = tabs + 1
			newtext = newtext + txt[i] + "\n"
			for i in range(tabs):
				newtext = newtext + "\t";
		elif txt[i] == "}":
			tabs = tabs-1
			for i in range(tabs):
				newtext = newtext + "\t";
			newtext = newtext + txt[i] + "\n"
		else:
			newtext = newtext + txt[i]

def tabOver(txt,tabs):
	if(tabs < 0):
		tabs = tabs * -1
		myre = re.compile("^[\t]{"+str(tabs)+"}")
		txt = re.sub(myre,"",txt)
		myre = re.compile("\n[\t]{"+str(tabs)+"}")
		txt = re.sub(myre,"\n",txt)
	else:
		for i in range(0,tabs):
			txt = re.sub("^(.)","\t\g<1>",txt);
			txt = re.sub("\n","\n\t",txt);
	return txt
	
def less10(i):
	if i < 10:
		return "0"+str(i)
	else:
		return str(i)
		
def ampersand(txt):
	rep = []
	for i in range(0,len(txt)):
		try:
			ch = ord(txt[i])
			if((ch < 32 or ch > 126) and ch != 10 and ch != 13 and ch != 9):
				rep.append(row[i])
		except:
			print("error ",sys.exc_info()[0].__str__)
			sys.stdin.readline()
	
	for rp in rep:
		rx = re.compile(rp);
		txt = rx.sub("&#"+less10(ord(rp))+";",txt)
	
	return txt

def csParam(txt):
	grps = txt.split(";")
	ntxt = ""
	for grp in grps:
		if(re.search("^\s*private\s",grp,re.I)):
			grp = re.sub("^\s*private\s+","",grp) #remove "private"
			tpe = re.findall("^[^ ]+",grp)[0] #find the <Type>
			grp = re.sub("^[^ ]+\s+","",grp) #Remove the <Type>
			varis = grp.split(",")
			for vari in varis:
				vari = vari.strip()
				pubname = "[name]"
				if re.search("^_",vari):
					pubname = re.sub("^_","",vari)
				else:
					pubname = vari
				firstLetter = pubname[0].upper()
				print("firstLetter: ",firstLetter)
				pubname = re.sub("^"+firstLetter.lower(),firstLetter,pubname)
				ntxt = ntxt + "public "+tpe+" "+pubname+" {\n\tget{ return this."+vari+";}\n\tset{this."+vari+" = value;}\n}\n"
	return ntxt

def Xls(txt,toHtml):
	if toHtml:
		txt = re.sub("&","&amp;",txt)
		txt = re.sub(">","&gt;",txt)
		txt = re.sub("<","&lt;",txt)
		txt = re.sub("\t","</td><td>",txt)
		txt = re.sub("\n","</td></tr>\n\t<tr><td>",txt)
		txt = re.sub("\\<td\\>\\s*?\\</td\\>","<td>&nbsp;</td>",txt)
		txt = re.sub("\n\t<tr><td>\\s*$","",txt)
		txt = re.sub("^\s*(?P<fc>[^\\<])","<tr><td>\g<fc>",txt)
		txt = re.sub("(?P<lc>[^\\>\n])\s*$","\g<lc></td></tr>",txt)
	else: #back to xls
		txt = re.sub(">\\s*<","><",txt)
		xmpat = re.compile("</(td|th|tr)>",2)
		txt = xmpat.sub("",txt)
		
		xmpat = re.compile("<tr[^>]*?><td[^>]*?>",2)
		txt = xmpat.sub("<tr>",txt)
		
		xmpat = re.compile("<tr[^>]*?>",2)
		txt = xmpat.sub("\n",txt)
		xmpat = re.compile("<td[^>]*?>",2)
		txt = xmpat.sub("\t",txt)
		
		txt = re.sub("&nbsp;"," ",txt)
		txt = re.sub("&gt;",">",txt)
		txt = re.sub("&lt;","<",txt)
		txt = re.sub("<[^>]+>","",txt)
		
	return txt

__main_usage__ = """Usage: clip [rep|tab|cap|grab|sort|comm|ht|sql|cnt|dedup|endl|xls|formatsql|tabright|csParams] [optional params]
rep: replaces regex param2 with string param3
tab: places int param2 tabs in front of all lines (takes negative values too)
cap: 1 = lower, 2 = caps, 3 = first letter
grab: finds all regex param2 and returns each to a line
sort: sorts based on string param2 defaults to \\n
comm: places string param2 in front of each line if param3 is not blank, removes param2 from each line
ht: changes url path to physical path, an optional param3=1 reverses
sql: no params; changes select statement into update statement
cnt: counts # of characters in text
dedup: removes duplicates line by line
endl: checks for c style syntax endings
xls: changes tab delimited text to html tables
formatsql: places new lines in sql for easier viewing
tabright: tabs over correctly following c style syntax (follow tabright with a number to add initial tabs, follow with the phrase "VB" for vb)
csParams: change "private <type> varname,varname,varname;" to "public <type> [name] {get{} set{}}"
"""
	
if __name__ == "__main__":
	if len(sys.argv) < 2:
		print(__main_usage__)
		blnk = sys.stdin.readline()
	else:
		txt = GetText()
		txt = re.sub("\r","",txt)
		txt = re.sub("\\r","",txt)
		txt = re.sub("\\n","\n",txt)
		if(len(sys.argv)>3 and sys.argv[1].lower()=="rep"):
			myre = re.compile(sys.argv[2],2)
			txt = eval("re.sub(myre,\""+sys.argv[3].replace("\"","\\\"")+"\",txt)")
		#---------------------------------------------------
		elif(len(sys.argv)>2 and sys.argv[1].lower()=="tab"):
			txt = tabOver(txt,int(sys.argv[2]))			
		#---------------------------------------------------
		elif(len(sys.argv)>2 and sys.argv[1].lower()=="cap"):
			if(sys.argv[2] == "1" or sys.argv[2].lower() == "l"):
				txt = txt.lower()
			if(sys.argv[2] == "2" or sys.argv[2].lower() == "u"):
				txt = txt.upper()
			if(sys.argv[2] == "3" or sys.argv[2].lower() == "ul"):
				txts = txt.split(" ")
				for i in range(0,len(txts)):
					txts[i] =  txts[i][0:1].upper() + txts[i][1:len(txt)].lower()
				txt = " ".join(txts)
		#---------------------------------------------------
		elif(len(sys.argv)>2 and (sys.argv[1].lower()=="grab" or sys.argv[1].lower()=="grep")):
			txt = "\n".join(re.findall(sys.argv[2],txt,2))
		#---------------------------------------------------
		elif(len(sys.argv)>1 and sys.argv[1].lower()=="sort"):
			sep = "\n"
			compare = 1;
			if(len(sys.argv)>2):
				sep = sys.argv[2]
				if(len(sys.argv)>3 and (sys.argv[3].lower()=="n" or sys.argv[3].lower()=="num")):
					compare = 0
			if(sep == "\\n"):
				sep = "\n"
			txts = txt.split(sep)
			if(compare == 0):
				txts.sort(key=int)
			else:
				txts.sort(key=lambda x: x.lower())
			remFromArr(txts,"")
			txt = sep.join(txts)
			#txt = re.sub(sep,"",txt,1)
		
		#---------------------------------------------------
		elif(len(sys.argv)>2 and sys.argv[1].lower()=="comm"):
			mycomm = sys.argv[2].replace("\"","\\\"")
			if len(sys.argv)>3:
				txt = re.sub("^"+mycomm,"",txt)
				txt = re.sub("\n"+mycomm,"\n",txt)
			else:
				txt = re.sub("^(.)",mycomm+"\\1",txt,2)
				txt = re.sub("\n","\n"+mycomm,txt)
		#---------------------------------------------------
		elif(len(sys.argv)>1 and sys.argv[1].lower()=="ht"):
			flc = open("clipht.dat","r")
			lns = flc.readlines()
			flc.close()
			if(len(sys.argv)>2 and sys.argv[2] == "1"):
				txt = re.sub("\\\\","/",txt)
				txt = re.sub(" ","%20",txt)
				for ln in lns:
					line = ln.split("\t")
					myre = re.compile("^"+re.sub("[\s\t\r\n]","",line[0]),2)
					txt = myre.sub(re.sub("[\s]","",line[1]),txt)
			else:
				txt = re.sub("%20"," ",txt)
				for ln in lns:
					line = ln.split("\t")
					myre = re.compile("^"+re.sub("[\s\t\r\n]","",line[1]),2)
					txt = myre.sub(re.sub("[\s]","",line[0]),txt)
				txt = re.sub("[#\?](.)*","",txt)
				myre = re.compile("/",re.I)
				txt = myre.sub("\\\\",txt)
		elif(len(sys.argv)>1 and sys.argv[1].lower()=="sql"):
			mysplitter = re.compile("(from|where)",re.I)
			mysplit = mysplitter.split(txt)
			txt = "UPDATE"+mysplit[2]+"\nSET \n"+mysplit[1]+mysplit[2]+mysplit[3]+mysplit[4]
		elif sys.argv[1].lower()=="cnt" or sys.argv[1].lower()=="count" or sys.argv[1].lower()=="len" or sys.argv[1].lower()=="length":
			print("Length is: "+str(len(txt)))
			blnk = sys.stdin.readline()
		elif sys.argv[1].lower()=="remdup" or sys.argv[1].lower()=="dedup":
			lns = txt.split("\n")
			i=0
			for ln in lns:
				i = i + 1
				for ln2 in lns[i:]:
					if ln2==ln:
						lns.remove(ln2)				
			txt = "\n".join(lns)
		elif sys.argv[1].lower()=="endl":
			#txt = re.sub("/\*[\d\D]+?\*/","",txt)
			txt = re.sub("//.*","",txt)
			hasErrs = False
			addLine = 1
			if(len(sys.argv)>2):
				addLine = int(sys.argv[2])
			lines = txt.split("\n")
			txt = ""
			myre = re.compile("[^}{;]$")
			bline = re.compile("^\s*$")
			i=0
			for i in range(0,len(lines)):
				if myre.search(re.sub("\s","",lines[i])) and not bline.search(lines[i]):
					hasErrs = True
					txt = txt + "check line "+str(i+addLine)+"\n"
			print(i)
			print(txt)
			if(hasErrs):
				print("done")
			else:
				print("clean")
			blnk = sys.stdin.readline()
		elif sys.argv[1].lower() == "xls":
			if len(sys.argv) > 2 and sys.argv[2].lower() == "html":
				txt = Xls(txt,False)
			else:
				txt = Xls(txt,True)
		elif sys.argv[1].lower() == "formatsql":
			myre = myre = re.compile("(?P<nl>inner\s+join|left\s+join|right\s+join|where|from|order by|group by|(?<=\s)and\s)",re.I)
			txt = myre.sub("\n\g<nl>",txt)
			txt = re.sub("\(","(\n\t",txt)
			txt = re.sub("\)","\n)",txt)
			#false positives off vv
			myre = re.compile("\(\n\tnolock\n\)",re.I)
			txt = myre.sub("(nolock)",txt)
			myre = re.compile("'(?P<dt1>[\d\-\/ \:APM]{8,})'\s+and\s+'(?P<dt2>[\d\-\/ \:APM]{8,})'",re.I)
			txt = myre.sub("'\g<dt1>' and '\g<dt2>'",txt)
		elif sys.argv[1].lower()=="dupbycol":
			colsToCheck = ["0"]
			if(len(sys.argv)>2):
				colsToCheck = sys.argv[2].split(",")
			
			checkedRows = []	
			rowsToCheck = txt.split("\n")
			for i in range(len(rowsToCheck)):
				row = rowsToCheck[i].split("\t")
				thisRowIsDup = False
				for j in range(len(checkedRows)):
					foundDup = True
					for k in colsToCheck:
						if len(checkedRows[j])<=int(k):
							foundDup = False
						elif len(row)<=int(k):
							foundDup = False
						elif checkedRows[j][int(k)].lower()!=row[int(k)].lower():
							foundDup=False
					if foundDup:
						thisRowIsDup=True
						break;
				if thisRowIsDup:
					print("row ",i," is a duplicate:",rowsToCheck[i])
				checkedRows.append(row)
			print("done")
			sys.stdin.readline()
		elif sys.argv[1].lower() == "tabright":
			if(len(sys.argv)>2):
				if(len(sys.argv)>3):
					n=0
					if(sys.argv[2].lower()=="vb"):
						n = int(sys.argv[3])
					if(sys.argv[3].lower()=="vb"):
						n = int(sys.argv[2])
					tabRightVB(txt,n)
				else:
					if(sys.argv[2].lower()=="vb"):
						txt = tabRightVB(txt)
					else:
						txt = tabRight(txt,int(sys.argv[2]))
			else:
				txt = tabRight(txt)
		elif sys.argv[1].lower() == "formatjs":
			txt = formatJS(txt)
		elif sys.argv[1].lower() == "ampersand" or sys.argv[1].lower() == "&":
			txt = ampersand(txt)
		elif sys.argv[1].lower() == "csparams" or sys.argv[1].lower() == "csparam":
			txt = csParam(txt)
		else:
			print(__main_usage__)
			blnk = sys.stdin.readline()
			
		txt = re.sub("\n","\r\n",txt)
		SetText(txt)
		#print(txt)
		

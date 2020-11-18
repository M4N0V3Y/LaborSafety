import { Injectable } from '@angular/core';
import * as js2xmlparser from 'js2xmlparser';
const XML_TYPE = 'text/xml;charset=utf-8';
const XML_EXTENSION = '.xml';
import * as FileSaver from 'file-saver';
import { ToasterService } from 'angular2-toaster';
import { IOptions } from 'js2xmlparser/lib/options';

@Injectable({
  providedIn: 'root'
})
export class XmlService {
  constructor(
    private toaster: ToasterService
  ) { }

  public SaveXmlFileFromJson(object: any, xmlFileName: string, rootName: string = 'item', options?: IOptions): void {
    const xmlParsed = js2xmlparser.parse(rootName, object,  options);

    if (xmlParsed) {
      this.saveAsXmlFile(xmlParsed, xmlFileName);
    } else {
      this.toaster.pop('error', '', 'Não foi possível exportar para XML');
    }
  }

  private saveAsXmlFile(buffer: any, fileName: string): void {
    const data: Blob = new Blob([buffer], { type: XML_TYPE });
    FileSaver.saveAs(data, fileName + XML_EXTENSION);
  }
}

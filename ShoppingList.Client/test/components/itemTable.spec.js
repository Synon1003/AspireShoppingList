import { describe, it, expect, beforeEach, afterEach } from 'vitest'
import { shallowMount } from '@vue/test-utils'
import ItemTable from '~/components/ItemTable.vue'

describe('ItemTable.vue', () =>
{
    let wrapper
    const items = [
        {
            id: 'guid-1',
            name: 'Apples',
            description: 'Fresh red apples',
            price: 300,
            updatedAt: "2025-12-06T21:21:21.1425398+00:00",
            status: 'Purchased',
        },
        {
            id: 'guid-2',
            name: 'Bananas',
            description: 'Ripe yellow bananas',
            price: 200,
            updatedAt: "2025-12-06T21:21:21.1425398+00:00",
            status: 'NotPurchased',
        },
    ]

    beforeEach(() => wrapper = null)

    afterEach(() =>
    {
        if (wrapper) wrapper.unmount()
    })

    it('renders table with the expected column headers', () =>
    {
        wrapper = shallowMount(ItemTable, {
            props: { items },
        })

        const headers = wrapper.findAll('thead th')
        const headerTexts = headers.map((th) => th.text())

        expect(headerTexts).toEqual([
            'Name',
            'Description',
            'Price',
            'Updated At',
            'Actions',
        ])
    })

    it('renders table rows with expected values', () =>
    {
        wrapper = shallowMount(ItemTable, {
            props: { items },
        })

        const rows = wrapper.findAll('tbody tr')
        expect(rows.length).toBe(2)

        const firstRowTds = rows[0].findAll('td')
            .map((td) => td.text())
        expect(firstRowTds).toContain('Apples')
        expect(firstRowTds).toContain('Fresh red apples')
        expect(firstRowTds).toContain('300')
        expect(firstRowTds).toContain('2025. 12. 06. 21:21:21')
        expect(firstRowTds).toContain('Purchased| Delete')

        const secondRowTds = rows[1].findAll('td')
            .map((td) => td.text())
        expect(secondRowTds).toContain('Bananas')
        expect(secondRowTds).toContain('Ripe yellow bananas')
        expect(secondRowTds).toContain('200')
        expect(firstRowTds).toContain('2025. 12. 06. 21:21:21')
        expect(secondRowTds).toContain('NotPurchased| Delete')
    })

    it('applies linethrough and gray background for "Beszerezve" status', () =>
    {
        wrapper = shallowMount(ItemTable, {
            props: { items },
        })

        const row = wrapper.findAll('tbody tr')[0]
        expect(row.classes()).toContain('line-through')
        expect(row.classes()).toContain('bg-base-300')
    })

    it('emits "toggleItemStatus" when status is clicked', async () =>
    {
        wrapper = shallowMount(ItemTable, {
            props: { items },
        })

        const statusBtn = wrapper.findAll('tbody tr')[0]
            .findAll('button')[0]
        await statusBtn.trigger('click')

        expect(wrapper.emitted().toggleItemStatus).toBeTruthy()
        expect(wrapper.emitted().toggleItemStatus[0]).toEqual(['guid-1'])
    })

    it('emits "deleteItem" when delete button is clicked', async () =>
    {
        wrapper = shallowMount(ItemTable, {
            props: { items },
        })

        const deleteBtn = wrapper.findAll('tbody tr ')[0]
            .findAll('button')[1]
        await deleteBtn.trigger('click')

        expect(wrapper.emitted().deleteItem).toBeTruthy()
        expect(wrapper.emitted().deleteItem[0]).toEqual(['guid-1'])
    })
})